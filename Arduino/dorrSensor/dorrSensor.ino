/*
 * 예제 20-1
 * 마그네틱 스위치로 시리얼통신을 이용해서 열림과 닫힘을 감지해보라!
 */
int mag_sensor = 0;
int LED = 4;
int speaker = 5;

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
    //달라붙어있는거 = 닫힘
    Serial.println("아! 닫혔네요!");
    digitalWrite(LED, HIGH);
  }else{
    //열림
    Serial.println("열렸습니다!");
    digitalWrite(LED, LOW);
    tone(speaker, 330, 250);
  }
  delay(100);
}