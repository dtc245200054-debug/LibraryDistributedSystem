# BÁO CÁO TUẦN 3 – GIAO TIẾP THỜI GIAN THỰC VỚI SIGNALR

## 1. Mục tiêu

- Tìm hiểu cơ chế giao tiếp thời gian thực.
- Tìm hiểu SignalR Hub và SignalR Groups.
- Xây dựng chức năng gửi thông báo trạng thái yêu cầu đặt mượn sách.
- Kết hợp REST API và SignalR trong hệ thống thư viện.

## 2. Công việc thực hiện

- Xây dựng ReservationDto.
- Xây dựng CreateReservationRequest.
- Xây dựng ReservationStatusMessage.
- Tạo ReservationHub để quản lý kết nối máy khách.
- Tạo ReservationGroups để phân nhóm theo mã yêu cầu.
- Đăng ký SignalR bằng AddSignalR().
- Ánh xạ Hub bằng MapHub().
- Xây dựng API tạo yêu cầu đặt mượn sách.
- Mô phỏng quá trình xử lý yêu cầu đặt mượn.
- Sử dụng IHubContext để gửi thông báo đến máy khách.
- Xây dựng Library.Client sử dụng SignalR Client để nhận thông báo.

## 3. Các API được xây dựng

| Phương thức | Endpoint | Chức năng |
|---|---|---|
| GET | /api/books | Lấy danh sách sách |
| GET | /api/reservations/{id} | Xem thông tin yêu cầu đặt mượn |
| POST | /api/books/{bookId}/reservations | Tạo yêu cầu đặt mượn |

## 4. Quy trình xử lý yêu cầu đặt mượn

1. Máy khách kết nối đến SignalR Hub.
2. Máy khách gửi yêu cầu đặt mượn thông qua REST API.
3. Máy chủ tạo yêu cầu với trạng thái received.
4. Máy khách đăng ký theo dõi yêu cầu.
5. Máy chủ cập nhật trạng thái thành processing.
6. Máy chủ gửi thông báo qua SignalR.
7. Máy chủ cập nhật trạng thái thành ready.
8. Máy chủ tiếp tục gửi thông báo qua SignalR.
9. Máy khách nhận thông báo và kiểm tra trạng thái cuối cùng bằng REST API.

## 5. Kết quả kiểm thử

| Nội dung | Kết quả |
|---|---|
| Kết nối REST API | Thành công |
| Kết nối SignalR Hub | Thành công |
| Tạo yêu cầu đặt mượn | Thành công – HTTP 201 |
| Nhận trạng thái processing | Thành công |
| Nhận trạng thái ready | Thành công |
| Kiểm tra trạng thái cuối | ready |
| Build solution | Thành công |

## 6. Công nghệ sử dụng

- C# và .NET 11.
- ASP.NET Core Minimal API.
- RESTful API.
- SignalR.
- HttpClient.
- Microsoft.AspNetCore.SignalR.Client.

## 7. Kết luận

Đã xây dựng được chức năng giao tiếp thời gian thực cho hệ thống thư viện và đặt mượn sách phân tán.

REST API được sử dụng để tạo và truy vấn yêu cầu đặt mượn. SignalR được sử dụng để gửi thông báo khi trạng thái yêu cầu thay đổi.

Qua kiểm thử, máy khách đã nhận được đầy đủ hai thông báo processing và ready.

Dữ liệu và quá trình xử lý hiện đang được mô phỏng trong bộ nhớ. Các tuần tiếp theo sẽ tiếp tục phát triển chức năng lưu trữ dữ liệu và xử lý phân tán.