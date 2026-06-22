-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 22, 2026 at 11:32 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `techgear`
--

-- --------------------------------------------------------

--
-- Table structure for table `korisnik`
--

CREATE TABLE `korisnik` (
  `korisnikID` int(11) NOT NULL,
  `email` varchar(100) NOT NULL,
  `lozinka` varchar(100) NOT NULL,
  `ime` varchar(50) DEFAULT NULL,
  `prezime` varchar(50) DEFAULT NULL,
  `uloga` varchar(20) DEFAULT 'Kupac'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `korisnik`
--

INSERT INTO `korisnik` (`korisnikID`, `email`, `lozinka`, `ime`, `prezime`, `uloga`) VALUES
(1, 'dinosrnec56@gmail.com', 'ZRsj4W8r', 'Dino ', 'Srnec', 'Admin'),
(2, 'matija.farkas24@yahoo.com', 'MatijaFarkas1298672', 'Matija', 'Farkas', 'Kupac'),
(4, 'josko.jozic@gmail.com', 'JasamJoza123', 'Josko ', 'Jozic', 'Kupac'),
(5, 'tonka.duranec@gmail.com', 'Tonka123', 'Tonka', 'Đuranec', 'Kupac'),
(6, 'jovan.jovanovic@yahoo.com', 'jovan123', 'Jovan', 'Jovanovic', 'Kupac');

-- --------------------------------------------------------

--
-- Table structure for table `narudzba`
--

CREATE TABLE `narudzba` (
  `NarudzbaID` int(11) NOT NULL,
  `KorisnikID` int(11) NOT NULL,
  `DatumNarudžbe` datetime(6) NOT NULL,
  `UkupnaCijena` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `narudzba`
--

INSERT INTO `narudzba` (`NarudzbaID`, `KorisnikID`, `DatumNarudžbe`, `UkupnaCijena`) VALUES
(1, 6, '2026-06-11 00:32:42.004340', 245.00),
(2, 6, '2026-06-11 00:34:17.264810', 245.00),
(3, 6, '2026-06-11 00:34:55.214975', 1748.99),
(4, 4, '2026-06-11 00:38:54.408388', 2964.00);

-- --------------------------------------------------------

--
-- Table structure for table `narudzba_stavka`
--

CREATE TABLE `narudzba_stavka` (
  `NarudzbaStavkaID` int(11) NOT NULL,
  `NarudzbaID` int(11) NOT NULL,
  `ProizvodID` int(11) NOT NULL,
  `Naziv` varchar(100) NOT NULL,
  `Cijena` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `narudzba_stavka`
--

INSERT INTO `narudzba_stavka` (`NarudzbaStavkaID`, `NarudzbaID`, `ProizvodID`, `Naziv`, `Cijena`) VALUES
(1, 1, 9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 245.00),
(2, 2, 9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 245.00),
(3, 3, 4, 'AMD Ryzen 7 7800X3D BOX', 499.99),
(4, 3, 5, 'ASUS ROG STRIX RTX 4080 Super', 1249.00),
(5, 4, 9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 245.00),
(6, 4, 5, 'ASUS ROG STRIX RTX 4080 Super', 1249.00),
(7, 4, 9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 245.00),
(8, 4, 9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 245.00),
(9, 4, 9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 245.00),
(10, 4, 9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 245.00),
(11, 4, 9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 245.00),
(12, 4, 9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 245.00);

-- --------------------------------------------------------

--
-- Table structure for table `proizvod`
--

CREATE TABLE `proizvod` (
  `proizvodID` int(11) NOT NULL,
  `naziv` varchar(100) NOT NULL,
  `opis` text DEFAULT NULL,
  `cijena` decimal(10,2) NOT NULL,
  `kategorija` varchar(50) DEFAULT NULL,
  `slikaUrl` varchar(255) DEFAULT NULL,
  `Lager` int(11) NOT NULL DEFAULT 10
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `proizvod`
--

INSERT INTO `proizvod` (`proizvodID`, `naziv`, `opis`, `cijena`, `kategorija`, `slikaUrl`, `Lager`) VALUES
(4, 'AMD Ryzen 7 7800X3D BOX', 'Najbolji gaming procesor na svijetu. 8 jezgri, 16 threadova i revolucionarna 3D V-Cache tehnologija koja pruža nevjerojatan broj sličica u sekundi u svim modernim igrama. Socket AM5.', 499.99, 'Procesori', 'https://www.adm.hr/slike/velike/amd-ryzen-7-7800x3d-box-42ghz-96mb-120w-am5-radeon-graphics--39951-092100012.webp', 12),
(5, 'ASUS ROG STRIX RTX 4080 Super', 'Premium grafička kartica s masivnim trostrukim hlađenjem i RGB osvjetljenjem. Pokreće sve igre u 4K rezoluciji na maksimalnim postavkama uz DLSS 3 i Ray Tracing.', 1249.00, 'Grafičke', 'https://iqcentar.hr/image/cache/catalog/vendor/0001359333-vga-as-strix-rtx4080s-o16g-gaming-800x800.jpg', 3),
(6, 'Logitech G Pro X Superlight 2 Lightspeed', 'Ultra-lagani bežični gaming miš težine samo 60 grama. Dolazi s novim HERO 2 senzorom i hibridnim optičko-mehaničkim prekidačima za e-sport profesionalce.', 149.50, 'Miševi', 'https://images.chipoteka.hr/image/cachewebp/catalog/products/178282-1063/mis-logitech-g-pro-x-superlight-2-lightspeed-bezicni-32000-dpi-bijeli-YI6HYW81K-1155x1155.webp', 22),
(7, 'Razer DeathAdder V3 Pro', 'Ergonomski bežični miš za desnoruke igrače. Opremljen Razer Focus Pro 30K optičkim senzorom i masivnim trajanjem baterije do 90 sati neprekidnog rada.', 139.99, 'Miševi', 'https://store.alnabaa.com/cdn/shop/files/razer-deathadder-v3-pro-wireless-mouse-uk-rz01-04630100-r3g1-3_800x_6ed9ee47-ef7b-4a73-a2b7-4c41ef8112a9.webp?v=1725361832&width=800', 12),
(8, 'Keychron Q1 Pro RGB Wireless Knob', 'Potpuno aluminijska bežična mehanička tipkovnica s Hot-swap podnožjima i programibilnim okretnim gumbom. Dolazi s tvornički podmazanim prekidačima.', 199.00, 'Tipkovnice', 'https://www.keychron.com/cdn/shop/products/Keychron-Q1-Pro-QMK-VIA-wireless-custom-mechanical-keyboard-knob-75-percent-layout-full-aluminum-white-frame-for-Mac-Windows-Linux-with-RGB-backlight-hot-swappable-K-Pro-switch-red.jpg?crop=center&height=1200&v=1', 10),
(9, 'ASUS ROG Azoth 75% Custom Gaming Keyboard', 'Premium 75% mehanička tipkovnica s ugrađenim OLED zaslonom, troslojnim prigušivanjem zvuka i silikonskim brtvama za ultimativni osjećaj tipkanja.', 245.00, 'Tipkovnice', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQVdv-QzCk-Cnkqfe0ZTrqPY_h2ox2wBGbGhw&s', 0),
(10, 'ASUS ROG Swift OLED PG27AQDM 27\"', 'Vrhunski 27-inčni gaming monitor rezolucije 1440p. OLED panel pruža beskonačan kontrast, savršenu crnu boju i ekstremno brz odziv od 0.03 ms uz 240Hz osvježavanje.', 899.00, 'Monitori', 'https://www.nabava.net/slike/products/85/53/45765385/asus-pg27aqdm_613b7537.jpeg', 4),
(11, 'LG UltraGear 27GR75Q-B IPS QHD', 'Best-buy monitor s IPS panelom, QHD rezolucijom (2560x1440), brzinom osvježavanja od 165Hz i vremenom odziva od 1ms GtG. Podržava G-Sync i FreeSync Premium.', 259.99, 'Monitori', 'https://www.nabava.net/slike/products/30/79/60547930/monitori-lg-ultragear-27gr75q-monitor-ips-27_e5efdf7b.webp', 12),
(12, 'Samsung 990 PRO 2TB NVMe PCIe 4.0', 'Vrhunski PCIe 4.0 NVMe SSD s brzinama čitanja do 7450 MB/s. Idealan za sistemski disk, zahtjevan video rendering i ekstremno brzo učitavanje igara.', 179.99, 'SSD', 'https://www.adm.hr/slike/velike/samsung-ssd-2tb-990-pro-m2-pcie-40-x4-mz-v9p2t0bw-1200tbw-2151-098700233.webp', 18),
(13, 'Crucial SSD 4TB P310, M.2 SSD, NVMe PCIe, Gen 4, CT4000P310SSD8', 'Pohrana (sučelje)	M.2 PCIe/NVMe\r\nPohrana (kapacitet)	4TB', 378.99, 'SSD', 'https://www.adm.hr/slike/velike/crucial-ssd-4tb-p310-m2-ssd-nvme-pcie-gen-4-ct4000p310ssd8-43036-098700407.webp', 13),
(14, 'Sapphire RX9070XT NITRO+ OC, 16GB GDDR6, AMD Radeon, 11348-01-20G', 'Vrhunska izvedba AMD-ove arhitekture s metalnim kućištem i moćnim ventilatorima. Nudi sjajne performanse na 1440p rezoluciji uz obilnih 16GB VRAM-a.', 973.60, 'Grafičke', 'https://www.adm.hr/slike/velike/sapphire-rx9070xt-nitro-oc-16gb-gddr6-amd-radeon-11348-01-20-76898-097200684.webp', 7),
(15, 'Intel Core i7-14700K Raptor Lake Refresh', 'Moćan hibridni procesor s 20 jezgri (8 performansnih i 12 učinkovitih) i 28 threadova. Izvrstan balans za profesionalni rad, streaming i gaming. LGA1700.', 443.34, 'Procesori', 'https://www.tonerpartner.hr/image/agint00003-654270fc9ae0f-847476-330-330-3.webp', 6),
(16, 'Lian Li O11 Dynamic EVO', 'Legendarno dvokomorno panoramsko kućište s kaljenim staklom. Nudi potpunu modularnost i savršen prostor za skrivanje kablova i postavljanje vodenih hlađenja.', 182.21, 'Kućišta', 'https://www.adm.hr/slike/velike/lian-li-midi-tower-o11-dynamic-evo-rgb-glass-window-black-o1-51541-100300971.webp', 15),
(17, 'Corsair 4000D Airflow', 'Minimalističko Midi-Tower kućište s prednjim panelom optimiziranim za maksimalan protok zraka. Sadrži bočni prozor od kaljenog stakla i sustav za organizaciju kablova RapidRoute.', 107.00, 'Kućišta', 'https://www.racunala.hr/slike/velike/CC-9011201-WW_1.jpg', 12),
(18, 'SteelSeries Arctis Nova Pro Wireless', 'Premium bežične gaming slušalice s aktivnim poništavanjem buke (ANC), vanjskim hi-res DAC-om i inovativnim sustavom dviju baterija koje se mijenjaju bez gašenja uređaja.', 360.00, 'Slušalice', 'https://www.hgspot.hr/image/catalog/slike/243710-878.jpg?v=1.1249827053', 9),
(19, 'HyperX Cloud III Wireless Red/Black', 'Evolucija najudobnijih slušalica na svijetu. Dolaze s masivnim trajanjem baterije do nevjerojatnih 120 sati, redizajniranim mikrofonom i legendarnom HyperX memorijskom pjenom.', 149.99, 'Slušalice', 'https://www.mikronis.hr/_shop/files/products/50918-2965_PastedPicture_2025_10_31_0937.jpg?preset=product-fullsize&id=2241985', 12),
(20, 'Xbox Wireless Controller Carbon Black', 'Službeni Microsoft bežični kontroler nove generacije. Karakteriziraju ga hibridni D-pad, teksturirani rukohvati na trigerima i nativna Bluetooth podrška za PC i konzole.', 75.99, 'Kontroleri', 'https://www.gamershop.hr/content/product_instances2/734003/xboxseries-xbox-series-x-wireless-controller-black_thumb320.webp', 13),
(21, 'PlayStation 5 DualSense Edge Wireless', 'Pro verzija PS5 kontrolera s podesivim trigerima, zamjenjivim modulima gljiva, stražnjim programibilnim polugama (paddles) i ugrađenim profilima za e-sport.', 199.99, 'Kontroleri', 'https://www.nabava.net/slike/products/42/99/39949942/oprema-za-konzole-sony-dualsense-edge_16cd1f06.jpeg', 8);

-- --------------------------------------------------------

--
-- Table structure for table `recenzija`
--

CREATE TABLE `recenzija` (
  `recenzijaID` int(11) NOT NULL,
  `korisnikIme` varchar(100) NOT NULL,
  `tekst` varchar(1000) NOT NULL,
  `ocjena` int(11) NOT NULL,
  `datumKreiranja` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `recenzija`
--

INSERT INTO `recenzija` (`recenzijaID`, `korisnikIme`, `tekst`, `ocjena`, `datumKreiranja`) VALUES
(2, 'Jovan', 'Spora dostava', 3, '2026-06-11 00:26:14');

-- --------------------------------------------------------

--
-- Table structure for table `stavke_korisnika`
--

CREATE TABLE `stavke_korisnika` (
  `stavkaID` int(11) NOT NULL,
  `korisnikID` int(11) DEFAULT NULL,
  `proizvodID` int(11) DEFAULT NULL,
  `kolicina` int(11) DEFAULT 1,
  `tip` enum('Kosarica','Wishlist') DEFAULT 'Kosarica'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `wishlist`
--

CREATE TABLE `wishlist` (
  `id` int(11) NOT NULL,
  `korisnikID` int(11) NOT NULL,
  `proizvodID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `wishlist`
--

INSERT INTO `wishlist` (`id`, `korisnikID`, `proizvodID`) VALUES
(6, 6, 9);

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20260610222806_DodajNarudzbe', '10.0.7');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `korisnik`
--
ALTER TABLE `korisnik`
  ADD PRIMARY KEY (`korisnikID`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indexes for table `narudzba`
--
ALTER TABLE `narudzba`
  ADD PRIMARY KEY (`NarudzbaID`),
  ADD KEY `FK_narudzba_korisnik_KorisnikID` (`KorisnikID`);

--
-- Indexes for table `narudzba_stavka`
--
ALTER TABLE `narudzba_stavka`
  ADD PRIMARY KEY (`NarudzbaStavkaID`),
  ADD KEY `FK_narudzba_stavka_narudzba_NarudzbaID` (`NarudzbaID`);

--
-- Indexes for table `proizvod`
--
ALTER TABLE `proizvod`
  ADD PRIMARY KEY (`proizvodID`);

--
-- Indexes for table `recenzija`
--
ALTER TABLE `recenzija`
  ADD PRIMARY KEY (`recenzijaID`);

--
-- Indexes for table `stavke_korisnika`
--
ALTER TABLE `stavke_korisnika`
  ADD PRIMARY KEY (`stavkaID`),
  ADD KEY `korisnikID` (`korisnikID`),
  ADD KEY `proizvodID` (`proizvodID`);

--
-- Indexes for table `wishlist`
--
ALTER TABLE `wishlist`
  ADD PRIMARY KEY (`id`),
  ADD KEY `korisnikID` (`korisnikID`),
  ADD KEY `proizvodID` (`proizvodID`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `korisnik`
--
ALTER TABLE `korisnik`
  MODIFY `korisnikID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `narudzba`
--
ALTER TABLE `narudzba`
  MODIFY `NarudzbaID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `narudzba_stavka`
--
ALTER TABLE `narudzba_stavka`
  MODIFY `NarudzbaStavkaID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `proizvod`
--
ALTER TABLE `proizvod`
  MODIFY `proizvodID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT for table `recenzija`
--
ALTER TABLE `recenzija`
  MODIFY `recenzijaID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `stavke_korisnika`
--
ALTER TABLE `stavke_korisnika`
  MODIFY `stavkaID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `wishlist`
--
ALTER TABLE `wishlist`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `narudzba`
--
ALTER TABLE `narudzba`
  ADD CONSTRAINT `FK_narudzba_korisnik_KorisnikID` FOREIGN KEY (`KorisnikID`) REFERENCES `korisnik` (`korisnikID`) ON DELETE CASCADE;

--
-- Constraints for table `narudzba_stavka`
--
ALTER TABLE `narudzba_stavka`
  ADD CONSTRAINT `FK_narudzba_stavka_narudzba_NarudzbaID` FOREIGN KEY (`NarudzbaID`) REFERENCES `narudzba` (`NarudzbaID`) ON DELETE CASCADE;

--
-- Constraints for table `stavke_korisnika`
--
ALTER TABLE `stavke_korisnika`
  ADD CONSTRAINT `stavke_korisnika_ibfk_1` FOREIGN KEY (`korisnikID`) REFERENCES `korisnik` (`korisnikID`) ON DELETE CASCADE,
  ADD CONSTRAINT `stavke_korisnika_ibfk_2` FOREIGN KEY (`proizvodID`) REFERENCES `proizvod` (`proizvodID`) ON DELETE CASCADE;

--
-- Constraints for table `wishlist`
--
ALTER TABLE `wishlist`
  ADD CONSTRAINT `wishlist_ibfk_1` FOREIGN KEY (`korisnikID`) REFERENCES `korisnik` (`korisnikID`),
  ADD CONSTRAINT `wishlist_ibfk_2` FOREIGN KEY (`proizvodID`) REFERENCES `proizvod` (`proizvodID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
