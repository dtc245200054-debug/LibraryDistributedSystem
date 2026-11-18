# BÁO CÁO TUẦN 2 – XÂY DỰNG RESTFUL API

## 1. Mục tiêu
- Tìm hiểu giao thức HTTP và mô hình yêu cầu – phản hồi.
- Nắm nguyên tắc thiết kế RESTful API.
- Sử dụng DTO để trao đổi dữ liệu.
- Xây dựng API bằng ASP.NET Core Minimal API.
- Sử dụng HttpClient để giao tiếp giữa Client và Server.

## 2. Công việc thực hiện
- Tạo BookDto và CreateBookRequest.
- Tạo ApiError để biểu diễn thông tin lỗi.
- Xây dựng API kiểm tra trạng thái dịch vụ.
- Xây dựng API lấy danh sách và chi tiết sách.
- Bổ sung chức năng tìm kiếm sách bằng query string.
- Xây dựng API thêm sách và kiểm tra dữ liệu đầu vào.
- Xử lý trường hợp ISBN bị trùng.
- Xây dựng Library.Client sử dụng HttpClient.
- Xử lý phản hồi thành công, phản hồi lỗi và lỗi kết nối.

## 3. Danh sách API

| Phương thức | Endpoint | Chức năng |
|---|---|---|
| GET | / | Kiểm tra trạng thái API |
| GET | /api/books | Lấy danh sách sách |
| GET | /api/books?keyword=Java | Tìm kiếm sách |
| GET | /api/books/{id} | Xem chi tiết sách |
| POST | /api/books | Thêm sách mới |

## 4. Kết quả kiểm thử

| Mã HTTP | Ý nghĩa |
|---|---|
| 200 | Lấy dữ liệu thành công |
| 201 | Thêm sách thành công |
| 400 | Dữ liệu không hợp lệ hoặc ISBN trùng |
| 404 | Không tìm thấy sách |

## 5. Kết luận
Đã xây dựng được RESTful API cơ bản cho hệ thống thư viện. Máy khách có thể gửi yêu cầu HTTP, nhận dữ liệu JSON, kiểm tra mã trạng thái và xử lý thông báo lỗi.

Dữ liệu hiện được lưu trong bộ nhớ để phục vụ thực hành. Các chức năng lưu trữ bền vững và xử lý phân tán sẽ được phát triển ở những tuần tiếp theo.