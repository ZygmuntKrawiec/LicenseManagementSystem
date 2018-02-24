# LicenseManagementSystem

A free interpretation of a challenge found on https://join.eset.com/en/challenges/cnet-developer (20180224).


The project contains:<br/>

**1. LicenseManagementSystemBusinessLayer** 
     - LicenseManagementSystemWebService.asmx - A webservice to handling a users login/logout proces and Licensedata manipulation.
     - LicenseManagementSystemDatabase.mdf - A database to store Licensedata, users data who have access to Licensedata.
     - Code (Classes):
       - Authorizer,
       - HashedPassword,
       - LicensesDataProvider,
       - UserDataProvider.

**2. LicenseManagementSystemPresentationLayer** - A simple ASP.NET WebForms GUI for users to login and manipulate the data.
     - Forms:
       - Login,
       - Registration,
       - Welcome.
      
