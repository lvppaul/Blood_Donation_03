
USE BloodDonationDB

-- Insert data vào bảng Role
INSERT INTO Role (name, is_Deleted) VALUES
('Admin', 0),
('Staff', 0),
('Member', 0);


-- Insert data vào bảng User
INSERT INTO [User] (name, blood_type, phone, email, password, address, role_id, is_Deleted) VALUES
('Nguyen Van A', 'O+', '0123456789', 'nguyenvana@email.com', 'password123', '123 Nguyen Trai, Q1, HCM', 1, 0),
('Tran Thi B', 'A+', '0987654321', 'tranthib@email.com', 'password123', '456 Le Loi, Q3, HCM', 2, 0),
('Le Van C', 'B+', '0111222333', 'levanc@email.com', 'password123', '789 Hai Ba Trung, Q1, HCM', 3, 0),
('Pham Thi D', 'AB+', '0444555666', 'phamthid@email.com', 'password123', '321 Vo Van Tan, Q3, HCM', 3, 0),
('Hoang Van E', 'O-', '0777888999', 'hoangvane@email.com', 'password123', '654 Nguyen Hue, Q1, HCM', 3, 0),
('Vo Thi F', 'A-', '0333444555', 'vothif@email.com', 'password123', '987 Dong Khoi, Q1, HCM', 3, 0),
('Dang Van G', 'B-', '0666777888', 'dangvang@email.com', 'password123', '147 Le Thanh Ton, Q1, HCM', 3, 0),
('Bui Thi H', 'AB-', '0999000111', 'buithih@email.com', 'password123', '258 Pasteur, Q3, HCM', 2, 0);

-- Insert data vào bảng StatusNotification
INSERT INTO StatusNotification (status_name, is_Deleted) VALUES
('Sent', 0),
('Read', 0),
('Pending', 0),
('Failed', 0);

-- Insert data vào bảng Notification
INSERT INTO Notification (user_id, message, type, status_notification_id, is_Deleted) VALUES
(3, 'Thank you for your blood donation!', 'Appreciation', 2, 0),
(4, 'Your blood request has been approved', 'Request Update', 2, 0),
(5, 'Reminder: You can donate blood again next week', 'Reminder', 1, 0),
(6, 'Emergency blood request in your area', 'Emergency', 1, 0),
(7, 'Your blood donation appointment confirmed', 'Appointment', 2, 0);

-- Insert data vào bảng StatusBloodRequest
INSERT INTO StatusBloodRequest (status_name, is_Deleted) VALUES
('Pending', 0),
('Approved', 0),
('Fulfilled', 0),
('Cancelled', 0),
('Rejected', 0);

-- Insert data vào bảng BloodRequest
INSERT INTO BloodRequest (user_id, blood_type, component_type, amount, urgency_level, status_request_id, fulfilled_date, is_Deleted) VALUES
(6, 'A+', 'Red Blood Cells', 2, 'Normal', 3, '2024-12-15 14:30:00', 0),
(7, 'B+', 'Plasma', 1, 'Emergency', 2, NULL, 0),
(6, 'O-', 'Platelets', 3, 'Emergency', 3, '2024-12-20 09:15:00', 0),
(7, 'AB+', 'Red Blood Cells', 1, 'Normal', 1, NULL, 0),
(6, 'O+', 'White Blood Cells', 2, 'Normal', 2, NULL, 0);

-- Insert data vào bảng StatusBloodDonor
INSERT INTO StatusBloodDonor (status_name, is_Deleted) VALUES
('Available', 0),
('Unavailable', 0),
('Recovery Period', 0),
('Medical Hold', 0);

-- Insert data vào bảng DonorAvailability
INSERT INTO DonorAvailability (user_id, status_donor_id, last_donation_date, recovery_reminder_date, available_date, is_Deleted) VALUES
(3, 2, null, '2024-11-15', '2024-08-17', 0),
(5, 2, null, '2024-12-20', '2024-09-22', 0),
(3, 1, '2024-08-16', '2025-05-11', '2025-02-10', 0),
(4, 3, null, '2025-10-01', '2025-07-03', 0),
(5, 1, '2024-09-21', '2025-06-03', '2025-03-05', 0),
(6, 3, null, '2025-09-21', '2025-06-23', 0);

-- Insert data vào bảng MedicalFacility
INSERT INTO MedicalFacility (name, address, phone, email, description, is_Deleted) VALUES
('Benh Vien Cho Ray', '201B Nguyen Chi Thanh, Q5, HCM', '02838554269', 'contact@choray.vn', 'Major public hospital in Ho Chi Minh City', 0),
('Benh Vien Dai Hoc Y Duoc', '215 Hong Bang, Q5, HCM', '02838552225', 'info@umc.edu.vn', 'University Medical Center', 0),
('Benh Vien Nhi Dong 1', '341 Su Van Hanh, Q10, HCM', '02838650222', 'contact@nhi1.org.vn', 'Children Hospital District 1', 0),
('Trung Tam Hien Mau', '118 Hong Bang, Q5, HCM', '02838567890', 'blood@redcross.vn', 'Blood Donation Center', 0),
('Phong Kham Da Khoa An Binh', '123 An Binh, Binh Thanh, HCM', '02835551234', 'info@anbinh.com', 'General Medical Clinic', 0);

-- Insert data vào bảng DonationHistory
INSERT INTO DonationHistory 
(user_id, request_id, facility_id, amount,donation_date, blood_type, component_type, status, created_date, confirmed_date, is_Deleted) VALUES
(3, null, 1, 20,'2024-08-16','O+', 'Red Blood Cells', 0, '2024-08-16', '2024-08-17', 0),
(5, null, 4, 20,'2024-09-21', 'O-', 'Red Blood Cells', 0, '2024-09-21', '2024-09-22', 0),
(3, null, 1, 10,'2025-02-09', 'O+', 'Plasma', 0, '2025-02-09', '2025-02-10', 0),
(4, null, 2, 30,'2025-07-02', 'AB+', 'Platelets', 0, '2024-07-02', '2024-07-03', 0),
(5, null, 3, 10,'2024-03-04', 'O-', 'Platelets', 0, '2024-03-04', '2024-03-05', 0),
(6, null, 2, 10,'2025-06-22', 'O-', 'Platelets', 0, '2025-06-22', '2024-06-22', 0);

-- Insert data vào bảng StatusBloodInventory
INSERT INTO StatusBloodInventory (status_name, is_Deleted) VALUES
('Available', 0),
('Reserved', 0),
('Expired', 0),
('Low Stock', 0),
('Out of Stock', 0);

-- Insert data vào bảng BloodInventory
INSERT INTO BloodInventory (facility_id, component_type, amount, expired_date, status_inventory_id, blood_type, is_Deleted) VALUES
(1, 'Red Blood Cells', 5000, '2026-01-30', 1, 'O+', 0),
(1, 'Red Blood Cells', 3000, '2026-02-15', 1, 'A+', 0),
(2, 'Plasma', 2500, '2026-01-25', 4, 'B+', 0),
(3, 'Platelets', 1500, '2026-01-20', 4, 'AB+', 0),
(4, 'Red Blood Cells', 4000, '2026-02-10', 1, 'O-', 0),
(1, 'White Blood Cells', 1000, '2026-01-15', 4, 'A-', 0),
(2, 'Plasma', 3500, '2026-02-05', 1, 'O+', 0),
(5, 'Platelets', 8000, '2026-01-12', 4, 'B-', 0),
(3, 'Red Blood Cells', 2200, '2026-01-28', 1, 'AB-', 0),
(4, 'Plasma', 4500, '2026-02-20', 1, 'O-', 0);

-- Insert data vào bảng BlogPost
INSERT INTO BlogPost (title, content, author_id, category, is_document, is_Deleted) VALUES
('Benefits of Blood Donation', 'Blood donation is one of the most valuable contributions you can make to society. Regular blood donation helps maintain good health for donors and saves lives of patients in need. This article discusses the various health benefits and social impact of blood donation...', 1, 'Health', 0, 0),
('Understanding Blood Types', 'There are four main blood types: A, B, AB, and O. Each can be positive or negative based on the Rh factor. Understanding blood compatibility is crucial for safe blood transfusions. This comprehensive guide explains blood typing and compatibility rules...', 2, 'Education', 0, 0),
('Emergency Blood Needs', 'During emergencies and disasters, the need for blood increases dramatically. Hospitals require immediate access to various blood components to treat trauma patients. This article highlights the importance of maintaining adequate blood reserves...', 1, 'Emergency', 1, 0),
('Donor Eligibility Guidelines', 'Not everyone can donate blood due to health and safety considerations. This article outlines the eligibility criteria for blood donors, including age requirements, weight limits, health conditions, and temporary deferrals...', 2, 'Guidelines', 0, 0),
('Blood Component Separation', 'Modern blood banking involves separating whole blood into different components: red blood cells, plasma, platelets, and white blood cells. Each component has specific uses and storage requirements. Learn about the process and applications...', 1, 'Medical', 1, 0);


PRINT 'Data insertion completed successfully!';