#include <Servo.h>

Servo servo;
int btn = 5;
int speaker = 6;
int mag_sensor = 7;
int LED = 8;
int moter = 9;
unsigned long preTime;

void setup(){
  Serial.begin(115200);
  servo.attach(moter);
  Serial.println("Start Control.");
  pinMode(btn, INPUT);         // 버튼
  pinMode(mag_sensor, INPUT_PULLUP);   // 마그네틱 센서
  pinMode(LED, OUTPUT);       // LED 
  pinMode(speaker, OUTPUT);   // 부저
}

void loop() {
  // put your main code here, to run repeatedly:
  int btnValue = digitalRead(5);

  if(btnValue == LOW){ // 버튼을 누르면
    servo.write(50); // 잠금
    Serial.println("아! 닫혔네요!");
    digitalWrite(LED, HIGH); // 불끄기
    preTime = millis(); // 닫히면 현재시간으로 초기화
  }
  else{
    servo.write(150);  // 잠금해제
    // 문 열렸을 때
    unsigned long nowTime = millis(); // 현재시간 저장
    Serial.println("열렸습니다!");
    digitalWrite(LED, LOW); // 불켜기
    if(nowTime - preTime >= 5000) { // 현재와 기준 시간차가 5초 이상이면
      preTime = nowTime; // 기준 시간을 현재시간으로 변경
      Serial.println("문이 장시간 열려있습니다! 닫아주세요");
      for(int i = 0; i < 5; i++){   // 알림을 5번 울림
        tone(speaker, 330, 250);
        delay(500);
      }
    }
  }
  
  delay(1000);
}