-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 29, 2026 at 01:22 PM
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
-- Database: `gadget_store_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `transactions`
--

CREATE TABLE `transactions` (
  `id` int(11) NOT NULL,
  `transaction_id` varchar(50) DEFAULT NULL,
  `product_name` text DEFAULT NULL,
  `total_price` decimal(10,2) DEFAULT NULL,
  `transaction_date` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transactions`
--

INSERT INTO `transactions` (`id`, `transaction_id`, `product_name`, `total_price`, `transaction_date`) VALUES
(1, '9959cdb3-f6b1-4f2a-b1e6-5731f94972fa', '2x PC, 32x Laptop', 33152.00, '2026-03-22 21:16:12'),
(2, 'dd6d32b7-e0c7-4092-9008-d14274dd8b4d', '67x Mouse', 1876.00, '2026-03-22 21:17:21'),
(3, '79f13489-7f15-401a-a191-264873ea29ac', '5x Headphones, 5x Mouse, 5x Keyboard', 784.00, '2026-03-22 21:59:11'),
(4, 'd07d2bc9-5d25-4f09-8206-c680d12295d3', '1x Mouse', 28.00, '2026-03-22 22:05:07'),
(5, '43965b48-c969-4a0e-ad2f-6cce28e2ed2f', '1x PC', 2240.00, '2026-03-22 22:08:42'),
(6, 'ed1c3497-7901-4122-b98c-ea6f9c9efe63', '69x PC', 154560.00, '2026-03-22 22:45:38'),
(7, 'b826849b-737d-4d0c-a2a1-6ad711aacf5f', '2x PC', 4480.00, '2026-03-29 19:18:11');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `transactions`
--
ALTER TABLE `transactions`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `transactions`
--
ALTER TABLE `transactions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
