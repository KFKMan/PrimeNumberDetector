
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

        X_self = list(map(int,[raw,e,n]))

        z_values = x_y[0].split(",")

        Z_self = list(map(int,z_values))

        X_data.append(X_self)
        Z_data.append(Z_self)

X_array = np.array(X_data)
Z_array = np.array(Z_data)

print(X_array)
print(Z_array)

#EarlyStopping ile eðitim ilerlemiyorsa durdurma ve eski modele dönme
#Batch ve Epoch sayýsý ile eðitim gücünü arttýrma/azaltma | Yüksek güç yeni veriye daha az uyum, düþük güç düþük oran demek !!
#Normalizasyon eklenebilir !
#Öðrenme oraný ayarlanabilir | düþük öðrenme oraný demek az dalgalanma, az deðiþim demek


model = Sequential()
model.add(Dense(64, input_dim=3, activation='relu'))  # 3 input (X1, X2, X3)
model.add(Dense(32, activation='relu'))  # Gizli katman
model.add(Dense(15, activation='linear')) # Max 15 çýktý katmaný

# Çýktý katmaný: Çýkýþ olarak Z'nin sayýsýna göre nöron sayýsýný belirliyoruz
#output_size = Y_array.shape[1]  # Çýktý boyutu (Z1, Z2, Z3 sayýsý)
#model.add(Dense(output_size))  # Çýktý katmaný, Z'nin tüm deðerlerini tahmin eder

model.compile(optimizer='adam', loss='mean_squared_error', metrics=['accuracy'])

# Modeli eðitme
model.fit(X_array, Z_array, epochs=500, batch_size=32, verbose=1)
#epoch => tekrar sayýsý
#verbose => detaylarý, eðitim sonuçlarýný göster



# Modeli test etme: Eðitim verileri üzerinde tahmin yapma
#Z_pred = model.predict(X_array)
# Tahmin edilen Z deðerlerini yazdýrma
#print(f'Tahmin edilen Z deðerleri:\n{Z_pred[:5]}')  # Ýlk 5 tahmini göster


model.save('model.h5') #Modeli kaydediyoruz lazým olabilir


        

