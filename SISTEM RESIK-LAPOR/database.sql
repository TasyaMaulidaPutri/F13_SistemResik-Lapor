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
ALTER PROCEDURE sp_GetMahasiswa
AS
BEGIN
    SELECT
        mahasiswa.NIM as NIM,
        mahasiswa.Nama as Nama,
        mahasiswa.JenisKelamin as JenisKelamin,
        mahasiswa.TanggalLahir as TanggalLahir,
        mahasiswa.Alamat as Alamat,
        mahasiswa.Foto as Foto,
		mahasiswa.KodeProdi,
        ProgramStudi.NamaProdi as NamaProdi
    FROM
        Mahasiswa
    JOIN
        ProgramStudi
    ON mahasiswa.KodeProdi = ProgramStudi.KodeProdi;
END

/*=====================================================
2. STORED PROCEDURE SELECT BY NIM
=======================================================*/
ALTER PROCEDURE sp_GetMahasiswaByNIM
@pNIM char(11)
AS
BEGIN
    SELECT
        mahasiswa.NIM,
        mahasiswa.Nama,
        mahasiswa.JenisKelamin,
        mahasiswa.TanggalLahir,
        mahasiswa.Alamat,
        mahasiswa.Foto as Foto,
		mahasiswa.KodeProdi,
        ProgramStudi.NamaProdi
    FROM
        Mahasiswa
    JOIN
        ProgramStudi
    ON mahasiswa.KodeProdi = ProgramStudi.KodeProdi
    WHERE mahasiswa.NIM = @pNIM;
END

/*======================================================
3. STORED PROCEDURE INSERT
========================================================*/
ALTER PROCEDURE sp_InsertMahasiswa
@pNIM char(11),
@pNama varchar(100),
@pAlamat varchar(200),
@pJenisKelamin char(1),
@pTanggalLahir datetime,
@pKodeProdi char(4),
@pFoto varbinary(max)
AS
BEGIN
    INSERT INTO Mahasiswa
    (NIM, Nama, Alamat, JenisKelamin, TanggalLahir, KodeProdi, TanggalDaftar, foto)
    VALUES
    (@pNIM, @pNama, @pAlamat, @pJenisKelamin, @pTanggalLahir, @pKodeProdi, GETDATE(), @pFoto);
END

/*========================================================
4. STORED PROCEDURE UPDATE
===========================================================*/
ALTER PROCEDURE sp_UpdateMahasiswa
@pNIM char(11),
@pNama varchar(100),
@pAlamat varchar(200),
@pJenisKelamin char(1),
@pTanggalLahir datetime,
@pKodeProdi char(4),
@pFoto varbinary(max)
AS
BEGIN
    UPDATE Mahasiswa
    SET Nama = @pNama, Alamat = @pAlamat, JenisKelamin = @pJenisKelamin,
        TanggalLahir = @pTanggalLahir, KodeProdi = @pKodeProdi, foto = @pFoto
    WHERE NIM = @pNIM;
END

/*=========================================================
5. STORED PROCEDURE DELETE
===========================================================*/
CREATE PROCEDURE sp_DeleteMahasiswa
	@NIM CHAR(11)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM Mahasiswa
	WHERE NIM = @NIM
END

/*============================================================
6. STORED PROCEDURE COUNT (OUTPUT PARAMETER)
===============================================================*/
CREATE PROCEDURE sp_CountMahasiswa
	@Total INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT @Total = COUNT(*) FROM Mahasiswa
END

/*===============================================================
STORED PROCEDURE DASHBOARD
=================================================================*/
CREATE PROCEDURE sp_DashBoard
AS
BEGIN
    SELECT
        ProgramStudi.NamaProdi as NamaProdi,
        count(mahasiswa.KodeProdi) as JmlMhs
    FROM
        Mahasiswa
    JOIN
        ProgramStudi
    ON mahasiswa.KodeProdi = ProgramStudi.KodeProdi
    GROUP BY ProgramStudi.NamaProdi;
END

CREATE PROCEDURE sp_DashBoardByTahun
    @inTglMsuk char(4)
AS
BEGIN
    SELECT
        ProgramStudi.NamaProdi as NamaProdi,
        count(mahasiswa.KodeProdi) as JmlMhs
    FROM
        Mahasiswa
    JOIN
        ProgramStudi
    ON mahasiswa.KodeProdi = ProgramStudi.KodeProdi
    WHERE YEAR(mahasiswa.TanggalDaftar) = @inTglMsuk
    GROUP BY ProgramStudi.NamaProdi;
END

/*===============================================================
Membuat Tabel Logging
=================================================================*/
CREATE TABLE LogError
(
	id_log INT IDENTITY(1,1) PRIMARY KEY,
	waktu DATETIME,
	pesan_error VARCHAR(MAX)
);

CREATE TABLE LogAktivitas
(
	id_log INT IDENTITY,
	aktivitas VARCHAR(100),
	waktu DATETIME
);

CREATE TRIGGER trg_InsertMahasiswa
ON Mahasiswa
AFTER INSERT
AS
BEGIN
	INSERT INTO LogAktivitas
	VALUES('Tambah data mahasiswa', GETDATE());
END;

select * from LogAktivitas
select * from LogError

CREATE TRIGGER trg_DeleteMahasiswa
ON Mahasiswa
AFTER DELETE
AS
BEGIN
	INSERT INTO LogAktivitas
	VALUES('Hapus data mahasiswa', GETDATE());
END;

CREATE TABLE LogKeamanan
(
	id_log INT IDENTITY(1,1),
	aktivitas VARCHAR(200),
	jumlah_data INT,
	waktu DATETIME
);

CREATE TRIGGER trg_PreventMassUpdate
ON Mahasiswa
AFTER UPDATE
AS
BEGIN
	DECLARE @jumlah INT;

	SELECT @jumlah = COUNT(*) FROM inserted;

	-- jika update lebih dari 5 data
	IF @jumlah > 5
	BEGIN
		-- Simpan log keamanan
		INSERT INTO LogKeamanan
		VALUES(
			'WARNING : Update massal terdeteksi',
			@jumlah,
			GETDATE()
		);

		--Membatalkan transaksi
		ROLLBACK TRANSACTION

		--Menampilkan pesan error
		RAISERROR(
		'Update dibatalkan! Terlalu banyak data diubah.',
		16,
		1
		);
	END
END;

CREATE PROCEDURE sp_Report
    @inProdi char(50),
    @inTglMsuk char(4)
AS
BEGIN

    SELECT
        mahasiswa.Nama as Nama,
        mahasiswa.JenisKelamin as JenisKelamin,
        mahasiswa.Alamat as Alamat,
        ProgramStudi.NamaProdi as NamaProdi,
        mahasiswa.TanggalDaftar as TanggalDaftar

    FROM
        Mahasiswa

    JOIN
        ProgramStudi

    ON mahasiswa.KodeProdi = ProgramStudi.KodeProdi

    WHERE
        ProgramStudi.NamaProdi = @inProdi
        AND YEAR(mahasiswa.TanggalDaftar) = @inTglMsuk;

END

alter table mahasiswa
add foto varbinary(max);

ALTER LOGIN sa WITH PASSWORD = 'tasya123';
ALTER LOGIN sa ENABLE;

EXEC sp_DashBoard

SELECT *
FROM sys.procedures
WHERE name = 'sp_LogMessage';

CREATE PROCEDURE sp_LogMessage
    @psm VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO LogError
    (
        waktu,
        pesan_error
    )
    VALUES
    (
        GETDATE(),
        @psm
    );
END
GO

EXEC sp_GetMahasiswa
select* from mahasiswa

SELECT * FROM Mahasiswa ORDER BY TanggalDaftar DESC;

SELECT * FROM ProgramStudi;