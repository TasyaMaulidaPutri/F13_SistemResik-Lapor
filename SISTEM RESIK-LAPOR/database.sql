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

CREATE VIEW vwLaporanPublic AS
SELECT
	id_laporan,
    id_user,
    deskripsi,
    foto,
    lokasi_maps,
    status
FROM Laporan;

SELECT *
INTO Laporan_Backup
FROM Laporan;

--stored procedure select
CREATE PROCEDURE sp_GetLaporan
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_laporan,
        id_user,
        deskripsi,
        foto,
        lokasi_maps,
        status
    FROM vwLaporanPublic
END

--stored procedure select parameter
CREATE PROCEDURE sp_GetLaporanById
    @id_laporan INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_laporan,
        id_user,
        deskripsi,
        foto,
        lokasi_maps,
        status
    FROM vwLaporanPublic
    WHERE id_laporan = @id_laporan
END

--Stroed procedure insert
CREATE PROCEDURE sp_InsertLaporan
    @id_user INT,
    @deskripsi VARCHAR(255),
    @foto VARCHAR(255),
    @lokasi_maps VARCHAR(255)
AS
BEGIN
    INSERT INTO Laporan
    (id_user, deskripsi, foto, lokasi_maps, status)
    VALUES
    (@id_user, @deskripsi, @foto, @lokasi_maps, 'lapor')
END

--Stored procedure update
CREATE PROCEDURE sp_UpdateLaporan
    @id_laporan INT,
    @deskripsi VARCHAR(255),
    @foto VARCHAR(255),
    @lokasi_maps VARCHAR(255),
    @status VARCHAR(50)
AS
BEGIN
    UPDATE Laporan
    SET
        deskripsi = @deskripsi,
        foto = @foto,
        lokasi_maps = @lokasi_maps,
        status = @status
    WHERE id_laporan = @id_laporan
END

--stored procedure delete
CREATE PROCEDURE sp_DeleteLaporan
    @id_laporan INT
AS
BEGIN
    DELETE FROM Laporan
    WHERE id_laporan = @id_laporan
END

--stored procedure search
CREATE PROCEDURE sp_SearchLaporan
    @keyword VARCHAR(255)
AS
BEGIN
    SELECT *
    FROM vwLaporanPublic
    WHERE
        deskripsi LIKE '%' + @keyword + '%'
        OR lokasi_maps LIKE '%' + @keyword + '%'
        OR CAST(id_laporan AS VARCHAR) LIKE '%' + @keyword + '%'
END

--Stored Procedure COUNT (OUTPUT PARAMETER)
CREATE PROCEDURE sp_CountLaporan
    @jumlah INT OUTPUT
AS
BEGIN
    SELECT @jumlah = COUNT(*) FROM Laporan
END

--====================================
-- CREATE VIEW
--===================================
CREATE VIEW vwSetoranPublic AS
SELECT
    id_setoran,
    id_user,
    berat_kg,
    nama_jenis_sampah,
    poin_per_kg,
    total_poin_setoran,
    status_verifikasi
FROM Setoran;
GO
-- =========================================
-- STORED PROCEDURE SELECT ALL SETORAN
-- =========================================
CREATE PROCEDURE sp_GetSetoran
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_setoran,
        id_user,
        berat_kg,
        nama_jenis_sampah,
        poin_per_kg,
        total_poin_setoran,
        status_verifikasi
    FROM vwSetoranPublic
END
GO


-- =========================================
-- STORED PROCEDURE SELECT SETORAN BY ID
-- =========================================
CREATE PROCEDURE sp_GetSetoranById
    @id_setoran INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_setoran,
        id_user,
        berat_kg,
        nama_jenis_sampah,
        poin_per_kg,
        total_poin_setoran,
        status_verifikasi
    FROM vwSetoranPublic
    WHERE id_setoran = @id_setoran
END
GO


-- =========================================
-- STORED PROCEDURE INSERT SETORAN
-- =========================================
CREATE PROCEDURE sp_InsertSetoran
    @id_user INT,
    @berat_kg INT,
    @nama_jenis_sampah VARCHAR(100),
    @poin_per_kg INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @total_poin INT

    SET @total_poin = @berat_kg * @poin_per_kg

    INSERT INTO Setoran
    (
        id_user,
        berat_kg,
        nama_jenis_sampah,
        poin_per_kg,
        total_poin_setoran,
        status_verifikasi
    )
    VALUES
    (
        @id_user,
        @berat_kg,
        @nama_jenis_sampah,
        @poin_per_kg,
        @total_poin,
        'pending'
    )
END
GO


-- =========================================
-- STORED PROCEDURE UPDATE SETORAN
-- =========================================
CREATE PROCEDURE sp_UpdateSetoran
    @id_setoran INT,
    @berat_kg INT,
    @nama_jenis_sampah VARCHAR(100),
    @poin_per_kg INT,
    @status_verifikasi VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @total_poin INT

    SET @total_poin = @berat_kg * @poin_per_kg

    UPDATE Setoran
    SET
        berat_kg = @berat_kg,
        nama_jenis_sampah = @nama_jenis_sampah,
        poin_per_kg = @poin_per_kg,
        total_poin_setoran = @total_poin,
        status_verifikasi = @status_verifikasi
    WHERE id_setoran = @id_setoran
END
GO


-- =========================================
-- STORED PROCEDURE DELETE SETORAN
-- =========================================
CREATE PROCEDURE sp_DeleteSetoran
    @id_setoran INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Setoran
    WHERE id_setoran = @id_setoran
END
GO


-- =========================================
-- STORED PROCEDURE SEARCH SETORAN
-- =========================================
CREATE PROCEDURE sp_SearchSetoran
    @keyword VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM vwSetoranPublic
    WHERE
        nama_jenis_sampah LIKE '%' + @keyword + '%'
        OR status_verifikasi LIKE '%' + @keyword + '%'
        OR CAST(id_setoran AS VARCHAR) LIKE '%' + @keyword + '%'
END
GO


-- =========================================
-- STORED PROCEDURE COUNT SETORAN
-- =========================================
CREATE PROCEDURE sp_CountSetoran
    @jumlah INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @jumlah = COUNT(*)
    FROM vwSetoranPublic
END
GO