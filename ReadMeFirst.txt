-----*****AUDIT TRAIL*****-----

Made in Dotnet Core 6.0

After Syncing Audit Trail
Open Nuget Package Manager Console and past below command,

1. ---dotnet ef migrations add InitialCreate --project AuditTrail/AuditTrail.csproj --startup-project AuditTrail/AuditTrail.csproj---
2. ---dotnet ef database update --project AuditTrail/AuditTrail.csproj---

Run below command if dotnet ef not installed.
---dotnet tool install --global dotnet-ef --version 6.*---

Now This project will give of changes object.

use below Post for checking

{
  "entityName": "User",
  "userId": "admin123",
  "action": "Updated",
  "objectBefore": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "status": "Active"
  },
  "objectAfter": {
    "id": 1,
    "name": "John Smith",
    "email": "john.smith@example.com", 
    "status": "Active"
  }
}