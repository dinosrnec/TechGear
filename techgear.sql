-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 06, 2026 at 11:16 PM
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
(1, 'dinosrnec56@gmail.com', 'adasdas', 'Dino ', 'Srnec', 'Kupac'),
(2, 'matija.farkas24@yahoo.com', 'MatijaFarkas1298672', 'Matija', 'Farkas', 'Kupac');

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
  `slikaUrl` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `proizvod`
--

INSERT INTO `proizvod` (`proizvodID`, `naziv`, `opis`, `cijena`, `kategorija`, `slikaUrl`) VALUES
(1, 'Geforce RTX 4080', 'Moćna grafička kartica za gaming.', 1200.00, 'Grafičke kartice', 'data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxMSEhUTEhMWFRUXFxUXFxcYFx0aGxsbGBsdFhcZHhkZHSggGB0lIBcYJjEhJSorLi4vGR8zODMtNygtMCsBCgoKDg0OGxAQGjcmICUuLS0vMjUtLS42LS0tLS0tLS0tLSstLS0rLSstLS0tLS0rKy8vLS0tLS0tKy4tLS0tLf/AABEIAOAA4AMBIgACEQEDEQH/'),
(2, 'Intel I5-9600 ', 'Intel I5-9600 (3.1GHz, 9MB, 65W, LGA 1151)', 125.32, 'Procesori', 'https://megabajt.hr/wp-content/uploads/2020/05/intel-core-i5-9600-procesor.jpg');

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
-- Indexes for table `proizvod`
--
ALTER TABLE `proizvod`
  ADD PRIMARY KEY (`proizvodID`);

--
-- Indexes for table `stavke_korisnika`
--
ALTER TABLE `stavke_korisnika`
  ADD PRIMARY KEY (`stavkaID`),
  ADD KEY `korisnikID` (`korisnikID`),
  ADD KEY `proizvodID` (`proizvodID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `korisnik`
--
ALTER TABLE `korisnik`
  MODIFY `korisnikID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `proizvod`
--
ALTER TABLE `proizvod`
  MODIFY `proizvodID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `stavke_korisnika`
--
ALTER TABLE `stavke_korisnika`
  MODIFY `stavkaID` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `stavke_korisnika`
--
ALTER TABLE `stavke_korisnika`
  ADD CONSTRAINT `stavke_korisnika_ibfk_1` FOREIGN KEY (`korisnikID`) REFERENCES `korisnik` (`korisnikID`) ON DELETE CASCADE,
  ADD CONSTRAINT `stavke_korisnika_ibfk_2` FOREIGN KEY (`proizvodID`) REFERENCES `proizvod` (`proizvodID`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
