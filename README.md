Description 📝

Feedback360 is a classic .NET MVC project for managing requests and feedback.
It uses SQL Server with stored procedures and provides dynamic data visualization with Chart.js.
The frontend leverages HTML, CSS, Bootstrap, JavaScript, and jQuery/AJAX for a responsive user experience.

Main Features ⚡
User and role management
Data visualization with Chart.js
CRUD operations using AJAX and jQuery
Responsive interfaces with Bootstrap
Database interaction via SQL Server and stored procedures

Technologies Used 🛠️
Backend: .NET Framework MVC (C#)
Database: SQL Server, stored procedures
Frontend: HTML, CSS, Bootstrap, JavaScript, jQuery, AJAX
Charts: Chart.js

Installation 💻
Clone the repository:
git clone https://github.com/PutMyUserName/Feedback360.git
Open the project in Visual Studio.
Restore NuGet packages if required.
Configure your SQL Server connection string in Web.config:
<connectionStrings>
    <add name="ConexionSql" providerName="System.Data.ProviderName" connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=Quejas; User='Quejas';Password='123*'" />
</connectionStrings>
Run the project using IIS Express or your preferred server.

Database 💾
A backup of the database is included in the Database folder.
Passwords are encrypted using:
UPDATE Usuario 
SET Password = ENCRYPTBYPASSPHRASE('quejas2025', '123*') 
WHERE Usuario = 'Conexion';
Test credentials:
User: Conexion
Password: 123*
Ensure all stored procedures are created and configured properly.

Configuration ⚙️
Forms Authentication is enabled in Web.config:
<authentication mode="Forms">
    <forms loginUrl="/Acceso/LogIn"></forms>
</authentication>
Other important settings in Web.config:
Patron for encryption: "quejas2025"
Client validation: true
Unobtrusive JavaScript: true
