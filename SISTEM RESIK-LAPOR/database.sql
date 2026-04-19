CREATE DATABASE DBResikLaporADO;
GO
USE DBResikLaporADO;
GO

-- ========================
-- 1. TABEL USERS (LOGIN)
-- ========================
CREATE TABLE Users (
    id_user INT IDENTITY(1,1) PRIMARY KEY,
    nama VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(100) NOT NULL,
    alamat VARCHAR(200),
    role VARCHAR(20) CHECK (role IN ('Admin','Masyarakat')) NOT NULL
);

-- ========================
-- 2. TABEL LAPORAN
-- ========================
CREATE TABLE Laporan (
    id_laporan INT IDENTITY(1,1) PRIMARY KEY,
    id_user INT NOT NULL,
    deskripsi VARCHAR(255),
    foto VARCHAR(255),
    lokasi_maps VARCHAR(255),
    status VARCHAR(20) CHECK (status IN ('lapor','proses','bersih')),

    CONSTRAINT FK_Laporan_User
        FOREIGN KEY (id_user) REFERENCES Users(id_user)
        ON DELETE CASCADE
);

-- ========================
-- 3. TABEL SETORAN
-- ========================
CREATE TABLE Setoran (
    id_setoran INT IDENTITY(1,1) PRIMARY KEY,
    id_user INT NOT NULL,
    berat_kg FLOAT,
    nama_jenis_sampah VARCHAR(100),
    poin_per_kg INT,
    total_poin_setoran INT,
    status_verifikasi VARCHAR(20) CHECK (status_verifikasi IN ('pending','verifikasi','ditolak')),

    CONSTRAINT FK_Setoran_User
        FOREIGN KEY (id_user) REFERENCES Users(id_user)
        ON DELETE CASCADE
);


-- USERS (LOGIN)
INSERT INTO Users (nama, email, password, alamat, role) VALUES
('Admin', 'admin@gmail.com', '123', 'Kampus', 'Admin'),
('Bassss', 'bas@gmail.com', '123', 'Yogyakarta', 'Masyarakat');

INSERT INTO Laporan (id_user, deskripsi, foto, lokasi_maps, status) VALUES
(1, 'Sampah menumpuk di pinggir jalan', 'foto1.jpg', 'Jl. Malioboro', 'lapor'),
(2, 'Banyak sampah plastik di selokan', 'foto2.jpg', 'Jl. Kaliurang', 'proses');

INSERT INTO Setoran (id_user, berat_kg, nama_jenis_sampah, poin_per_kg, total_poin_setoran, status_verifikasi) VALUES
(1, 2.5, 'Plastik', 1000, 2500, 'pending'),
(2, 1.0, 'Kertas', 800, 800, 'verifikasi');

SELECT * FROM Users

