/*
 * 예제 20-1
 * 마그네틱 스위치로 시리얼통신을 이용해서 열림과 닫힘을 감지해보라!
 */
int mag_sensor = 0;
int LED = 4;
int speaker = 5;
unsigned long preTime;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  pinMode(2, INPUT_PULLUP);   // 마그네틱 센서
  pinMode(LED, OUTPUT);       // LED 
  pinMode(speaker, OUTPUT);   // 부저
}

void loop() {
  // put your main code here, to run repeatedly:
  mag_sensor = digitalRead(2);
  if(mag_sensor == LOW){
    // 문이 닫혔을 때
    Serial.println("아! 닫혔네요!");
    digitalWrite(LED, HIGH); // 불끄기
    preTime = millis(); // 닫히면 현재시간으로 초기화
  }else{
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