# 🏥 MediCare+ - Smart Healthcare Management System

MediCare+ is a robust ASP.NET Core MVC application tailored for healthcare clinics and hospitals. It helps manage doctors, patients, appointments, checkups, and more, all while maintaining a clean and scalable codebase structure.

This system uses **CLEAN Architecture** and follows the **Repository Pattern** to ensure that business rules, infrastructure, and presentation are well-separated and maintainable.

---

## 🚀 Features

- 👨‍⚕️ Doctor management
- 👩‍💼 Receptionist scheduling
- 🧑‍🦰 Patient registration and history
- 📅 Appointment booking and management
- 📝 Doctor's CheckUp Notes
- ✉️ **Email Notification to Patients**
- ⬇️ Download Notes (PDF)
- 🔐 Role-based Access (Admin, Doctor, Receptionist)
- 📊 Clean, responsive UI with Bootstrap 5

---

## 🧱 Tech Stack

| Layer           | Tech Used                          |
|-----------------|------------------------------------|
| Frontend        | Razor Pages, Bootstrap 5, jQuery   |
| Backend         | ASP.NET Core MVC (.NET 7)          |
| Database        | SQL Server, Entity Framework Core  |
| Architecture    | CLEAN Architecture                 |
| Pattern         | Repository Pattern, Dependency Injection |
| Email Service   | SMTP Client (MailKit or System.Net.Mail) |

---

## 🧠 Architecture: CLEAN & Repository Pattern

### 🧼 CLEAN Architecture Overview
**Layer Responsibilities**:
- **Presentation**: Handles UI components and user interactions
- **Application**: Contains use cases, DTOs, and service implementations
- **Domain**: Core business entities and logic
- **Infrastructure**: Data persistence and external service integrations

## ⚙️ Setup Instructions
### ✅ Prerequisites
  - .NET 7 SDK
  - Visual Studio 2022+
  - SQL Server
  - SMTP Email Account (e.g., Gmail, Outlook)

## 🧪 Local Setup
**1. Clone the Repository**
   ```bash
   https://github.com/Utsav-7/MediCare_Full_Stack_MVC_Project.git
   cd MediCare_Full_Stack_MVC_Project
   ```
**2. Update Configuration**
  Update appsettings.json:
  ```bash
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MediCareDB;Trusted_Connection=True;"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "SenderName": "MediCare+",
    "SenderEmail": "your-email@gmail.com",
    "Password": "your-app-password"
  }
  ```
**3. Apply Migrations**
  ```bash
  dotnet ef database update
  ```
**4. Run the App**
  ```bash
  dotnet run
  ```
- Open in browser: https://localhost:5049

## 👥 Role-Based Access

| Role         | 👮 Permissions                            |
|--------------|-------------------------------------------|
| 🧑⚕️ Admin    | Full access to doctors, appointments, notes |
| 👨⚕️ Doctor   | Add/Edit patient notes, view own appointments |
| 💁 Receptionist | Book appointments, manage patients & schedules |

## 📧 SMTP Setup (Gmail Example)
- Enable 2FA in Gmail
- Create App Password and use it in appsettings.json
- Use port 587 with TLS for MailKit or System.Net.Mail

## 📸 Screenshots
![MediCare_Home-Page](https://github.com/user-attachments/assets/12866f0a-12df-4a54-9b19-50fc52538628)
![MediCare_Login-Page](https://github.com/user-attachments/assets/3ac8ed0a-c1fe-47cd-a034-83b2892657ec)
![MediCare_Contact-Page](https://github.com/user-attachments/assets/0d1dfd29-f540-44dc-ba16-7c73ca818338)



## 🤝 Contributing
1. Fork the project
2. Create a new branch (git checkout -b feature/YourFeature)
3. Commit your changes (git commit -am 'Add new feature')
4. Push to the branch (git push origin feature/YourFeature)
5. Open a pull request

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
