# Profile Core API

<https://api.adamhilty.com>

A backend web API built with .NET and C#. Supports a [GPA Calculator](https://github.com/robbobthecorncob1/GpaCalculator) web application and my [personal website](https://adamhilty.com).

## About

### Tech Stack

* **Framework:** .NET 10.0 / C# 14
* **Database:** SQLite
* **Email Forwarding:** Resend
* **Hosting/Deployment:** AWS Elastic Beanstalk
* **Security:** AWS CloudFront (Traffic restricted via Security Groups)

## Key Features

* **GPA Calculation Engine:** Processes grades, credit hours, and a target GPA to return accurate cumulative GPA data.
* **Portfolio Data Management:** Serves details about my projects, work experience, and technical skills.
* **Email Forwarding::** Incorporates [Resend](https://resend.com/) email service to securely send messages to a desired email.
* **Modern C# Patterns:** Leverages the latest C# features, including records with primary constructors for immutable data models, init-only properties for null-safety, and concise collection expressions.

## Local Development Setup

To run this API locally on your machine, follow these steps:

### Prerequisites

* [.NET 10 SDK](https://dotnet.microsoft.com/download) installed on your machine.
* Resend account and API key.

### 1. Clone the repo

```bash
git clone https://github.com/robbobthecorncob1/ProfileCore
cd ProfileCore
```

### 2. Configure CORS

You must add the service you are communicating with to appsettings.Development.json:

```bash
"AllowedOrigins": [
    "http://YOUR-URL-HERE:PORT"
  ],
```

### 3. Configure Resend

If you wish to use the Resend email service, you must enter your API key and receiver email in appsettings.Development.json:

```bash
},
    "Resend": {
    "ApiKey": "PUT API KEY HERE",
    "ToEmail": "PUT RECEIVER EMAIL HERE"
  }
```

### 4. Run the API

To run the API you must first enter the ProfileCore subfolder and then run the .NET project:

```bash
(cd ProfileCore && dotnet run)
```

## API Endpoints

### Website

GET  /api/website/profile - Fetches the owner's name, their bio, and links to their socials.  
GET  /api/website/experience - Fetches the complete work experience.  
GET  /api/website/projects - Fetches the complete list of projects.  
GET  /api/website/skills - Fetches the complete list of technical skills.  
GET  /api/website/education - Fetches the complete education history.  
GET  /api/website/status - Retrieves the current operational status of the API  
GET  /api/website/courses - Fetches the complete list of notable courses  
POST /api/website/contact - Processes and sends a message using an email forwarding service.

### GPA Calculator

POST /api/gpa/calculate-gpa - Calculates a cumulative GPA based on a provided list of courses, credits, and grades.  
POST /api/gpa/calculate-target - Calculates the required GPA for a future block of classes to achieve a specific target GPA.

## Deployment

This API is hosted on AWS Elastic Beanstalk.  
To ensure maximum security, the EC2 instances running the API are placed in a locked-down AWS Security Group. They do not accept direct public web traffic; they only accept requests routed through my custom AWS CloudFront distribution.

Production URL: <https://api.adamhilty.com>

## Author

Adam Hilty

B.S. Computer Science and Engineering  
The Ohio State University (May 2026)
