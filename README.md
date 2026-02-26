# Automated Event Access Control & Data Sync System

A professional ASP.NET Core solution designed for real-time guest validation and automated data synchronization between local MSSQL databases and Google Cloud (Sheets).

## 🚀 Overview
This system was built to manage high-traffic event entries. It features a custom QR validation engine that operates over a local LAN for speed and reliability, integrated with a post-event synchronization module to update master attendance records in the cloud.

## 🛠 Technical Stack & Integrations
* **Backend:** ASP.NET Core (MVC)
* **Database:** Microsoft SQL Server (MSSQL) with Entity Framework Core
* **Excel Processing:** ClosedXML
* **Cloud Integration:** Google Sheets API & Google Apps Script
* **Frontend UI:** JavaScript, HTML5, SweetAlert2 for real-time status feedback

## 🏗 Key Modules (Code Logic)

### 1. Real-Time QR Validation Engine
The `QrCodeController` handles incoming scan data via encrypted identifiers.
* **Security Filtering:** Implements server-side pattern matching to validate QR authenticity before database processing.
* **Concurrency & Duplicate Prevention:** Utilizes LINQ to cross-reference `existingQrCodeData` in MSSQL, preventing "double-entry" fraud with instant response codes.
* **Timestamp Logging:** Automatically records `Koha_skanimit` (scan time) for every successful entry.

### 2. Cloud-to-Local Data Sync
A specialized module designed to bridge the gap between registration (Google Forms) and attendance.
* **Automated Export:** Fetches live `.xlsx` response files directly from Google Drive/Sheets via `WebClient`.
* **ClosedXML Processing:** Parses large Excel datasets, dynamically updating guest statuses to "Pjesmarres" (Participant) based on local scan logs.
* **Smart State Management:** Includes flags to detect if updates are necessary, reducing unnecessary write operations to the file system.

### 3. Real-Time Client-Side Scanning
The frontend leverages the device's camera to process video frames locally, reducing server load and ensuring low latency.
* **Stream Management:** Uses `navigator.mediaDevices.getUserMedia` with a focus on `environment` facing mode for optimized mobile scanning.
* **In-Browser Decoding:** Integrated **jsQR** to capture and decode QR data from a hidden canvas element at 2.2s intervals to prevent accidental multi-scans.
* **Asynchronous UX:** Implemented **AJAX (jQuery)** for seamless data submission and real-time table updates without page reloads.
* **Modern UI Alerts:** Integrated **SweetAlert2** to provide instant visual feedback on scan status (success, duplicate, or invalid format).

## 💻 Technical Highlights from the Controller
* **Custom Status Codes:** Uses specific HTTP status codes (503, 504) to communicate complex validation errors to the frontend.
* **Environment Interaction:** Programmatically interacts with the host's file system to manage downloads and file paths (`Environment.SpecialFolder`).
* **Error Handling:** Implemented `try-catch` blocks to ensure system stability during IO operations and database transactions.

## 🛠 Setup
1. Clone the repository.
2. Configure your MSSQL connection string in `appsettings.json`.
3. Ensure the `ClosedXML` and `Google.Apis` NuGet packages are restored.
4. Deploy to a local server; access the scanner UI via any mobile device on the same LAN.
