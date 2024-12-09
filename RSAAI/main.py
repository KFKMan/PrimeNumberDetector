import numpy as np
import tensorflow as tf

import keras as krs
from keras.api.models import Sequential
from keras.api.layers import Dense
from keras.api.preprocessing.sequence import pad_sequences
from keras.api.optimizers import Adam
from keras.api.callbacks import EarlyStopping


#from tensorflow.keras.models import Sequential
#from tensorflow.keras.layers import Dense
#from tensorflow.keras.preprocessing.sequence import pad_sequences
#from tensorflow.keras.optimizers import Adam
#from tensorflow.keras.callbacks import EarlyStopping

print(tf.__version__)

dbfile = "analyze.rdb"

X_data = []
Z_data = []


with open(dbfile,"r") as file:
    lines = file.readlines()

    for line in lines:
        x_y = line.split(";");

        sp = x_y[0].split(",")

        raw = sp[0]
        e = sp[1]
        n = sp[2]

        if x_y[1].replace(" ","").replace("\n","").replace("\r","") == "":
            continue

        X_self = list(map(int,[raw,e,n]))

        z_values = x_y[1].split(",")

        Z_self = list(map(int,z_values))

        X_data.append(X_self)
        Z_data.append(Z_self)

X_array = np.array(X_data)

Z_padded_data = pad_sequences(Z_data, padding='post', dtype=np.int32)

Z_tensor = tf.convert_to_tensor(Z_padded_data, dtype=np.int32)
#Z_array = np.array(Z_data) #dtype=object for dynamic size

#print(X_array)
#print(Z_array)

#EarlyStopping ile eğitim ilerlemiyorsa durdurma ve eski modele dönme
#Batch ve Epoch sayısı ile eğitim gücünü arttırma/azaltma | Yüksek güç yeni veriye daha az uyum, düşük güç düşük oran demek !!
#Normalizasyon eklenebilir !
#Öğrenme oranı ayarlanabilir | düşük öğrenme oranı demek az dalgalanma, az değişim demek


model = Sequential()
model.add(Dense(64, input_dim=3, activation='elu'))  # 3 input (X1, X2, X3)
model.add(Dense(64, activation='elu'))
model.add(Dense(64, activation='elu')) # Gizli katman
model.add(Dense(Z_padded_data.shape[1], activation='linear')) # Max 200 çıktı katmanı => Z_padded_data büyüklüğü ile eşit çıktı katmanı

# Çıktı katmanı: Çıkış olarak Z'nin sayısına göre nöron sayısını belirliyoruz
#output_size = Y_array.shape[1]  # Çıktı boyutu (Z1, Z2, Z3 sayısı)
#model.add(Dense(output_size))  # Çıktı katmanı, Z'nin tüm değerlerini tahmin eder

optimizer = Adam(learning_rate=0.0001)
model.compile(optimizer=optimizer, loss='mean_squared_error', metrics=['accuracy'])

early_stopping = EarlyStopping(monitor='loss', patience=250, restore_best_weights=True)
# Modeli eğitme
model.fit(X_array, Z_tensor, epochs=100000, batch_size=64, verbose=1,callbacks=[early_stopping])
#epoch => tekrar sayısı
#verbose => detayları, eğitim sonuçlarını göster


# Modeli test etme: Eğitim verileri üzerinde tahmin yapma
#Z_pred = model.predict(X_array)
# Tahmin edilen Z değerlerini yazdırma
#print(f'Tahmin edilen Z değerleri:\n{Z_pred[:5]}')  # İlk 5 tahmini göster


model.save('model.keras') #Modeli kaydediyoruz lazım olabilir


        

