Form Koneksi: <img width="1053" height="609" alt="WhatsApp Image 2026-04-20 at 03 50 38" src="https://github.com/user-attachments/assets/66a52d75-7c78-4e42-b81f-9245b4a59549" />
From Input Data: <img width="1130" height="674" alt="WhatsApp Image 2026-04-20 at 05 54 49" src="https://github.com/user-attachments/assets/dbbad917-af9f-447f-b181-811d09b9fd9a" />
Form Input Data: <img width="1182" height="681" alt="WhatsApp Image 2026-04-20 at 05 56 03" src="https://github.com/user-attachments/assets/2259f713-8d0d-4727-9f17-9d8f398b1702" />
Form Tampilan Data: <img width="1142" height="668" alt="WhatsApp Image 2026-04-20 at 05 57 28" src="https://github.com/user-attachments/assets/4e0be0c9-2f50-49c4-a687-22589c76a2c3" />
Form Tampilan Data: <img width="1175" height="682" alt="WhatsApp Image 2026-04-20 at 05 58 26" src="https://github.com/user-attachments/assets/edf2de35-e353-4ec8-82b5-c547097fd8a8" />
Form CRUD: 
> Create: <img width="1144" height="673" alt="WhatsApp Image 2026-04-20 at 06 01 20" src="https://github.com/user-attachments/assets/afa82006-2b7e-40f4-ae5c-737f51386425" />
> Read: <img width="1135" height="675" alt="WhatsApp Image 2026-04-20 at 06 02 17" src="https://github.com/user-attachments/assets/a90b5895-dee6-4511-bda4-b11850f8cb9b" />
> Update: <img width="1137" height="675" alt="WhatsApp Image 2026-04-20 at 06 03 35" src="https://github.com/user-attachments/assets/a9f52815-df48-477b-8e1d-784b99d87999" />
> Delete: <img width="1140" height="673" alt="image" src="https://github.com/user-attachments/assets/677be601-48e5-4ea1-840e-99d3ff9d6e09" />

# Skenario SQL Injection pada Form Laporan
## Form yang Diserang
Form Laporan (Form2)
## Lokasi Kerentanan
Button Inject (`button4_Click_1`)

Kode rentan:
string query = "UPDATE Laporan SET deskripsi='HACKED' WHERE deskripsi='" + txtDeskripsi.Text + "'";

## Penyebab Kerentanan
Query SQL dibuat menggunakan penggabungan string (`string concatenation`) sehingga input pengguna langsung masuk ke query tanpa validasi atau parameterisasi.
Hal ini memungkinkan penyerang menyisipkan perintah SQL tambahan (SQL Injection).

# Simulasi Serangan SQL Injection
## Input Penyerang

Pada textbox `txtDeskripsi`, penyerang memasukkan:
' OR 1=1 --

## Query yang Terbentuk

UPDATE Laporan 
SET deskripsi='HACKED' 
WHERE deskripsi='' OR 1=1 --'

## Dampak Serangan
Karena kondisi `OR 1=1` selalu bernilai TRUE, maka seluruh data pada tabel `Laporan` akan diubah menjadi:
deskripsi = HACKED
Akibatnya:
* Semua laporan berubah
* Integritas data rusak
* Data asli hilang
---

# Dampak Keamanan
SQL Injection dapat menyebabkan:
* Manipulasi data
* Penghapusan data
* Pencurian data
* Bypass login
* Kerusakan database

---
# Cara Pencegahan

Gunakan `Parameterized Query` agar input user tidak dianggap sebagai perintah SQL.
Contoh aman:
string query = "UPDATE Laporan SET deskripsi='HACKED' WHERE deskripsi=@desk";
SqlCommand cmd = new SqlCommand(query, conn);
cmd.Parameters.AddWithValue("@desk", txtDeskripsi.Text);

Dengan parameterized query, input pengguna akan diperlakukan sebagai data biasa sehingga SQL Injection tidak dapat dilakukan.
