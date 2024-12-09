import keras as krs
from keras.api.models import load_model
import numpy as np

print("Model yükleniyor...")
loaded_model = load_model('model.keras')
print("Model yüklendi")

while True:
    x1 = int(input("X1 değerini girin: "))
    x2 = int(input("X2 değerini girin: "))
    x3 = int(input("X3 değerini girin: "))

    x_data = np.array([x1,x2,x3])

    predictions = load_model.predict(x_data)

    print("Sonuçlar;")
    print(predictions)