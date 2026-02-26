# Automated Event Access Control System (QR-Based)

A full-stack access control solution designed to automate guest registration and real-time entry validation for events using a hybrid architecture (Cloud + Local LAN).

## 🚀 Overview
This project was developed to manage event entries efficiently. It combines the scalability of Google Cloud tools for registration with a high-performance ASP.NET Core backend for local, real-time validation, ensuring 100% uptime even without an external internet connection during the event.

## 🛠 Tech Stack
* **Backend:** ASP.NET Core
* **Database:** Microsoft SQL Server (MSSQL)
* **Frontend:** HTML5, CSS3, JavaScript (SweetAlert2 for real-time UI/UX)
* **Automation:** Google Apps Script (GAS)
* **Integration:** Google Forms & Google Sheets API
* **Network:** Local Area Network (LAN) deployment via mobile hotspot

## ✨ Key Features
* **Automated Registration:** Guests register via Google Forms; a custom Google Apps Script generates a unique QR code and dispatches it via email automatically.
* **Security & Integrity:** Guest data is embedded within the QR code. The system uses **MSSQL** to track scan logs, preventing "double-entry" and unauthorized access.
* **LAN-Based Validation:** The ASP.NET Core server runs locally, allowing any mobile device (iOS/Android) on the same network to act as a high-speed scanner.
* **Data Synchronization:** Post-event module to import cloud registration data and cross-reference it with local MSSQL logs for final attendance analytics and timestamps.

## 🗄️ Database Logic (MSSQL)
The system utilizes MSSQL to handle:
* **Attendance Tracking:** Real-time logging of scanned QR codes.
* **Concurrency Control:** Ensuring that a QR code cannot be validated twice simultaneously.
* **Relational Mapping:** Matching local scan IDs with imported guest credentials for final reporting.



## 💻 Installation & Setup
1. Clone the repository.
2. Update the connection string in `appsettings.json` to point to your **MSSQL** instance.
3. Run `Update-Database` to apply migrations (if using Entity Framework).
4. Deploy to a local server and connect scanners via the LAN IP.
