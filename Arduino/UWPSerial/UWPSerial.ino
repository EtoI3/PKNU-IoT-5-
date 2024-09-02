#include <Servo.h>

Servo servo;
int mag_sensor = 10;
int moter = 9;
int led = 8;
int speaker = 7;

int value = 0;
unsigned long preTime;

void setup() {
  servo.attach(moter);
  pinMode(mag_sensor, INPUT_PULLUP);   // 마그네틱 센서
  pinMode(led, OUTPUT);       // LED 
  pinMode(speaker, OUTPUT);   // 부저
  Serial.begin(9600);
}

void loop() {
  // 시리얼 통신, 서보모터 제어부분
  preTime = millis(); // 현재시간으로 초기화
  if (Serial.available() > 0) {
    char command = Serial.read();
    if (command == '1') {
      value = 0;
      servo.write(value); // 문열림
      delay(500);
      unsigned long nowTime = millis(); // 현재시간 저장
      digitalWrite(led, LOW); // 불켜기
      if(mag_sensor == LOW){
        if(nowTime - preTime >= 5000) { // 현재와 기준 시간차가 5초 이상이면
          preTime = nowTime; // 기준 시간을 현재시간으로 변경
          for(int i = 0; i < 5; i++){   // 알림을 5번 울림
            tone(speaker, 330, 250);
            delay(500);
          }
        }
      }
    } else if (command == '2') {
      value = 179;
      servo.write(value);
      delay(500);
      digitalWrite(led, HIGH); // 불끄기
    }
  }
}