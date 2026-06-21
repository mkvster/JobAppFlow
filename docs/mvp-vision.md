# JobAppFlow MVP Vision

## 1. Core Problem

JobAppFlow is a single-user web application designed to bring order to a high-volume software engineering job search.

During a job search, opportunities come from many sources, making it difficult to track application status, salary discussions, follow-ups, notes, and related documents.

The MVP focuses on turning messy job-posting information into structured, searchable, and trackable job application records.

The primary goal is not to build a full recruiting platform or a multi-user SaaS product. The goal is to create a practical self-hosted tool for managing one person’s job search workflow, built on a modern C#, ASP.NET Core, Angular, Azure, and AI-assisted architecture.

## 2. MVP Capabilities

2.1 JobAppFlow allows the user to paste raw job posting text copied from LinkedIn or similar job boards and convert it into a structured job record.

2.2 The system uses an LLM to extract structured job information from pasted job posting text, and allows the user to review and edit the extracted data before saving it. Tag support may also be introduced at this stage to improve organization and filtering, but the exact scope and implementation are TBD.

2.3 The system allows the user to track job opportunities by status, priority, source, company, role, salary range, location, workplace type, and application progress.

2.4 The system allows the user to store external AI review notes, such as match analysis, risks, salary suggestions, follow-up advice, or application decision notes generated outside the application. Using external AI tools keeps the MVP simpler and avoids the added complexity and cost of built-in AI analysis.

2.5 The system allows the user to attach files to each job application, including screenshots, PDFs, resumes, cover letters, confirmation pages, recruiter messages, and other supporting documents.

2.6 The system allows the user to view, search, and filter saved job applications and their attachments so that active opportunities, follow-ups, rejections, skipped roles, and priority targets are easier to manage. The system also allows the user to search for relevant information within stored attachments where supported.

2.7 The system allows the user to detect or flag possible duplicate job postings based on saved job records.

2.8 The system allows the user to run the application as a single-user online tool deployed to Azure.

## 3. Product Direction

JobAppFlow should remain focused on one practical workflow:

paste messy job posting text
-> extract structured job information
-> review and save the record
-> track status and notes
-> attach supporting documents
-> search and manage opportunities

The MVP should prioritize daily usefulness over platform complexity.

Technical architecture, deployment options, authentication design, and database workflow are described in separate documents.

### Technology stack

C#, ASP.NET Core Web API, Entity Framework Core, SQL Server, Azure SQL Database, Angular 21, Angular Material Design, TypeScript, REST API, OpenAPI / Swagger, Azure Blob Storage, Azure App Service Linux, Azure Application Insights, OpenAI API / Azure OpenAI, LLM-based structured data extraction, GitHub Actions, CI/CD, Docker, Azure Container Apps Consumption, authentication and authorization, prompt engineering, data modeling, and full-stack web development. Extraction, Full-Stack Web Development.
