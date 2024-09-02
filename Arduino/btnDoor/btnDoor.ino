#include <Servo.h>

Servo servo;
int btn = 5;
int speaker = 6;
int mag_sensor = 7;
int LED = 8;
int moter = 9;
int flag = 0;
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
    flag += 1;
  }
  if((flag % 2) == 0){ // 버튼을 두번 누를때마다
    servo.write(50); // 잠금
    Serial.println("아! 닫혔네요!");
    digitalWrite(LED, HIGH); // 불끄기
  }
  else{
    servo.write(150);  // 잠금해제
    // 문 열렸을 때
    Serial.println("열렸습니다!");
    digitalWrite(LED, LOW); // 불켜기
  }
  
  delay(1000);
}