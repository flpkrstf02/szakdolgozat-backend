!pip install tf2onnx
!pip install onnxmltools

from keras.models import load_model
import keras
import os
import json
from PIL import Image
import base64
from io import BytesIO
from keras.preprocessing.image import ImageDataGenerator
from keras import callbacks
from onnxmltools import convert_keras
import onnx

model = load_model('stagedetector.keras')

json_file_path = 'response_1696873837355.json'

output_directory = '/train'

with open(json_file_path, 'r') as json_file:
    data = json.load(json_file)

for entry in data:
    class_name = entry['prediction']
    class_directory = os.path.join(output_directory, class_name)
    os.makedirs(class_directory, exist_ok=True)

for entry in data:
    base64_data = entry['image']
    class_name = entry['prediction']

    image_data = base64.b64decode(base64_data)
    image = Image.open(BytesIO(image_data))

    class_directory = os.path.join(output_directory, class_name)
    image_path = os.path.join(class_directory, f'image_{len(os.listdir(class_directory)) + 1}.png')
    image.save(image_path)

train_gen = ImageDataGenerator(rescale=1./255,horizontal_flip=True,vertical_flip=True,rotation_range=90)
train_data = train_gen.flow_from_directory("/train/",
                                           target_size=(224, 224),
                                           batch_size=32,
                                           class_mode='categorical')

for layer in model.layers[:-5]:
    layer.trainable = False

model.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
model.build((64, 224, 224, 3))
history = model.fit(train_data, batch_size=32, epochs=1, verbose=1)

model.save('stagedetector.keras')
onnx_model = convert_keras(model)
onnx.save(onnx_model, 'stagedetector.onnx')