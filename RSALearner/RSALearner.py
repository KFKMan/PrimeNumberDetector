
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

#EarlyStopping ile e�itim ilerlemiyorsa durdurma ve eski modele d�nme
#Batch ve Epoch say�s� ile e�itim g�c�n� artt�rma/azaltma | Y�ksek g�� yeni veriye daha az uyum, d���k g�� d���k oran demek !!
#Normalizasyon eklenebilir !
#��renme oran� ayarlanabilir | d���k ��renme oran� demek az dalgalanma, az de�i�im demek


model = Sequential()
model.add(Dense(64, input_dim=3, activation='relu'))  # 3 input (X1, X2, X3)
model.add(Dense(32, activation='relu'))  # Gizli katman
model.add(Dense(15, activation='linear')) # Max 15 ��kt� katman�

# ��kt� katman�: ��k�� olarak Z'nin say�s�na g�re n�ron say�s�n� belirliyoruz
#output_size = Y_array.shape[1]  # ��kt� boyutu (Z1, Z2, Z3 say�s�)
#model.add(Dense(output_size))  # ��kt� katman�, Z'nin t�m de�erlerini tahmin eder

model.compile(optimizer='adam', loss='mean_squared_error', metrics=['accuracy'])

# Modeli e�itme
model.fit(X_array, Z_array, epochs=500, batch_size=32, verbose=1)
#epoch => tekrar say�s�
#verbose => detaylar�, e�itim sonu�lar�n� g�ster



# Modeli test etme: E�itim verileri �zerinde tahmin yapma
#Z_pred = model.predict(X_array)
# Tahmin edilen Z de�erlerini yazd�rma
#print(f'Tahmin edilen Z de�erleri:\n{Z_pred[:5]}')  # �lk 5 tahmini g�ster


model.save('model.h5') #Modeli kaydediyoruz laz�m olabilir


        

