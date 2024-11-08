# 📦 Smart Box : 스마트 보관함 IoT 앱 개발
> 아파트 등의 공동주거 형태가 아닌 주택가 등은 택배 분실이 잦음 <br>
> 지역 내 공동 보관함을 통해 택배물품 보관 및 수거의 안전성을 높임
<br>

## 📄 프로젝트 목표
- 사용자가 디스플레이를 통해 정보를 입력하여 물건을 안전하게 보관
- 애플리케이션을 통해 인증번호를 확인하여 보관함 열기 가능
<br>

## 🔥 프로젝트 과정
### 🔗 [요구사항 리스트](https://github.com/user-attachments/files/16600283/_2.3.xlsx)
### 🔗 [기능 명세]()
### 🔗 [Figma]()
### 🔗 [중간발표 PPT]()
<br>

## 🛠️ 기술스택
![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=flat-square&logo=Csharp&logoColor=white)
![C++](https://img.shields.io/badge/C++-%2300599C.svg?style=flat-square&logo=C%2B%2B&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=flat-square&logo=.net&logoColor=white)
![MAUI](https://img.shields.io/badge/MAUI-00008B?style=flat-square&logo=MAUI&logoColor=white)
![Firebase](https://img.shields.io/badge/Firebase-a08021?style=flat-square&logo=firebase&logoColor=ffcd34)
![Figma](https://img.shields.io/badge/figma-%23F24E1E.svg?style=flat-square&logo=figma&logoColor=white)
![Arduino](https://img.shields.io/badge/-Arduino-00979D?style=flat-square&logo=Arduino&logoColor=white)
![Raspberry Pi](https://img.shields.io/badge/-RaspberryPi-C51A4A?style=flat-square&logo=Raspberry-Pi)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=flat-square&logo=Visual-Studio&logoColor=white)
<br><br>

## 🧑‍💻역할분담
||이름|역할|수행업무|
|---|-----|---|----|
|<img src="https://github.com/EtoI3/PKNU-IoT-5-/blob/main/imgs/jw.png" height="150" width="140">|이주원  &nbsp;&nbsp;&nbsp;&nbsp;|[팀장] <br> - 프로젝트 매니저 <br> - 하드웨어 엔지니어|- 전반적인 프로젝트 관리 및 일정 관리 <br> - Raspberry Pi 또는 Arduino를 활용한 시스템 구현 <br> - 디스플레이 모듈 및 센서 연동 및 설정 <br> - 라즈베리파이의 디스플레이 모듈과의 호환성 검토 및 통합|
|<img src="https://github.com/EtoI3/PKNU-IoT-5-/blob/main/imgs/jh.png" height="150" width="140">|이주희|[팀원] <br> - 소프트웨어 개발자 <br> - 하드웨어 엔지니어|- 데이터베이스 (Firebase) 연동 및 데이터 관리 기능 개발 <br> -  보관함 상태 모니터링, 알림 기능 등의 앱 기능 구현 <br> - 보관함 잠금 비밀번호 확인 및 사용자 인증 기능 개발 <br> - 디스플레이 모듈 및 센서 연동 및 설정|
|<img src="https://github.com/EtoI3/PKNU-IoT-5-/blob/main/imgs/ij.png" height="150" width="140">|김인재|[팀원] <br> - 소프트웨어 개발자 <br> - 하드웨어 엔지니어|- 데이터베이스 (Firebase) 연동 및 데이터 관리 기능 개발 <br> -  보관함 상태 모니터링, 알림 기능 등의 앱 기능 구현 <br> - 보관함 잠금 비밀번호 확인 및 사용자 인증 기능 개발 <br> - 디스플레이 모듈 및 센서 연동 및 설정|
|<img src="https://github.com/EtoI3/PKNU-IoT-5-/blob/main/imgs/oj.png" height="150" width="140">|오혜진|[팀원] <br> - 모바일 앱 개발자 <br> - UI/UX 디자이너|- MAUI을 이용한 클로스 플랫폼 애플리케이션 개발 <br> - 사용자 인터페이스 개발 및 사용성 향상에 중점을 둔 기능 구현 <br> - 사용자 경험 및 인터페이스 디자인 담당 <br> - 모바일 앱과 웹 인터페이스의 시각적 및 기능적 디자인 개발 |
|<img src="https://github.com/EtoI3/PKNU-IoT-5-/blob/main/imgs/hj.png" height="150" width="140">|김현지|[팀원] <br> - 소프트웨어 개발자 <br> - UI/UX 디자이너|- UWP을 이용한 클로스 플랫폼 애플리케이션 개발 <br> - 사용자 인터페이스 개발 및 사용성 향상에 중점을 둔 기능 구현 <br> - 사용자 경험 및 인터페이스 디자인 담당 <br> - 모바일 앱과 웹 인터페이스의 시각적 및 기능적 디자인 개발|
<br>

## 📌 주요기능
#### 소프트웨어 부분
- UWP
- maui

#### 하드웨어 부분
- 라즈베리파이 / 아두이노
  - 물품보관함 시스템의 중앙 제어 장치로 사용
- 라즈베리파이 터치스크린 디스플레이
  - UWP의 인터페이스 창을 화면에 출력
<br>

## 💻 UI 화면 설명
#### 스마트폰 앱 📱(.NET MAUI)
<img width="300px" src="https://github.com/user-attachments/assets/11f344c1-e31e-4cff-af9a-8414bf325314"/>|<img width="300px" src="https://github.com/user-attachments/assets/eada39a6-ae48-4520-9e9f-db8c202f221a"/>|<img width="300px" src="https://github.com/user-attachments/assets/14eb8f76-2648-4862-82f0-9786e0929103"/>|
|:---:|:---:|:---:|
|🔼 로그인 페이지|🔼 메뉴 페이지|🔼 보관함 인증번호 표시|
|<img width="300px" src="https://github.com/user-attachments/assets/65ed4164-8c84-47b2-8184-e5f2fc713e23"/>|<img width="300px" src="https://github.com/user-attachments/assets/28b710ff-a614-4047-99c8-dbb9d517bd73"/>
|🔼 보관함 위치 표시|🔼 문의사항 페이지|

#### 라즈베리파이 터치스크린 📺(Window 10 Iot Core, UWP)
|<img width="500px" height="auto" src="https://github.com/user-attachments/assets/604cb9e2-c67b-443d-8a71-4a6d6b42012e"/>|<img width="500px" height="auto" src="https://github.com/user-attachments/assets/09e9511d-c50b-41f5-a8ef-e946946e1b16"/>|<img width="500px" height="auto" src="https://github.com/user-attachments/assets/05959cc4-a175-4138-8b21-876e9fc9df05"/>|
|:---:|:---:|:---:|
|🔼 메인 페이지|🔼 사용자유형 선택|🔼 발송인 번호입력|
|<img width="500px" height="auto" src="https://github.com/user-attachments/assets/e2e2dad6-574d-4c0b-bd34-6114040256cc"/>|<img width="500px" height="auto" src="https://github.com/user-attachments/assets/b6e7ed34-c2ac-4fb4-b7dc-0e6c9f81feb8"/>|<img width="500px" height="auto" src="https://github.com/user-attachments/assets/712eed93-1f00-4aa2-9a43-8d69bf8dd9cc"/>
|🔼 수령인 인증번호 입력|🔼 보관함 번호 선택|🔼 물품 보관 안내창|
|<img width="247px" height="auto" src="https://github.com/user-attachments/assets/0b51a6ea-9765-43b0-a407-d1f4cdae569f"/>|
|🔼 물품 회수 안내창|

<br>

## 🔎 프로젝트 구성도
<img src="https://github.com/EtoI3/PKNU-IoT-5-/blob/main/imgs/프로젝트 구성도.png" height="450" width="800">

## 🎥 프로젝트 최종
### 구현 모습

|<img height="500" width="300px" src="https://github.com/EtoI3/PKNU-IoT-5-/blob/main/imgs/정면.jpg"/>|<img height="500" width="300px" src="https://github.com/EtoI3/PKNU-IoT-5-/blob/main/imgs/측면.jpg"/>
|:---:|:---:|
|🔼 정면 모습|🔼 측면 모습|

#### 물건 보관/찾기

https://github.com/user-attachments/assets/e928b5d4-b421-4cff-9b79-9d7e50515b88

#### 센서 동작 확인

https://github.com/user-attachments/assets/297fbb4e-6eee-4a78-84fa-ba84098f2ec9
