# Automated Event Access Control & Data Sync System

A robust ASP.NET Core platform built for immediate guest authentication and automated data transfer between local MSSQL databases and Google Cloud Sheets.

## 🚀 Overview

This system was built to manage high-traffic event entries. It features a custom QR validation engine that runs over a local LAN for speed and reliability, integrated with a post-event synchronization module that updates master attendance records in the cloud.

## 🛠 Technical Stack & Integrations

* **Backend: ASP.NET Core (MVC)
* **Database: Microsoft SQL Server (MSSQL) with Entity Framework Core
* **Excel Processing: ClosedXML
* **Cloud Integration: Google Sheets API & Google Apps Script
* **Frontend UI: JavaScript, HTML5, SweetAlert2 for real-time status feedback

## 🏗 Key Modules (Code Logic)

### 1. Real-Time QR Validation Engine

The QrCodeController processes scan data using encrypted identifiers.

* **Security Filtering: Applies server-side pattern analysis to verify QR authenticity before database operations.
* **Concurrency & Duplicate Prevention: Uses LINQ to cross-check existing QR entries in MSSQL, preventing duplicate scans via immediate response codes.
* **Timestamp Logging: Logs Koha_skanimit (scan time) for each valid entry automatically.

### 2. Cloud-to-Local Data Sync

A purpose-built module linking digital registration (Google Forms) and on-site attendance.

* **Automated Export: Fetches live .xlsx response files directly from Google Drive/Sheets via WebClient.
* **ClosedXML Processing: Parses large Excel datasets, dynamically updating guest statuses to “Pjesmarres” (Participant) based on local scan logs.
* **Smart State Management: Uses flags to identify needed updates, minimizing redundant file writes.

### 3. Real-Time Client-Side Scanning

The frontend uses the device camera to locally scan video frames, reducing server load and ensuring low latency.

* **Stream Management: Uses navigator.mediaDevices.getUserMedia focuses on environment-facing mode for optimized mobile scanning.
* **In-Browser Decoding: Integrated jsQR to capture and decode QR data from a hidden canvas element at 2.2s intervals to prevent accidental multi-scans.
* **Asynchronous UX: Implemented AJAX (jQuery) for seamless data submission and real-time table updates without page reloads.
* **Modern UI Alerts: Integrated SweetAlert2 to provide instant visual feedback on scan status (success, duplicate, or invalid format).

## 💻 Technical Highlights from the Controller

* **Custom Status Codes: Uses specific HTTP status codes (503, 504) to communicate complex validation errors to the frontend.
* **Environment Interaction: Programmatically interacts with the host’s file system to manage downloads and file paths (Environment.SpecialFolder).
* **Error Handling: Implemented try-catch blocks to ensure system stability during IO operations and database transactions.

## 🛠 Setup
1. Clone the repository.
2. Configure your MSSQL connection string in appsettings.json.
3. Ensure the ClosedXML and Google. Apis NuGet packages are restored.
4. Deploy to a local server and access the scanner UI from any mobile device on the same LAN.
