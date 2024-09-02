#include <Servo.h>

Servo servo;
int moter = 9;
int ledPin = 8;
int value = 0;

void setup() {
  pinMode(ledPin, OUTPUT);
  servo.attach(moter);
  Serial.begin(9600);
}

void loop() {
  if (Serial.available() > 0) {
    char command = Serial.read();
    if (command == '1') {
      digitalWrite(ledPin, HIGH); // Turn LED on
      value = 0;
      servo.write(value);
      delay(500);
    } else if (command == '2') {
      digitalWrite(ledPin, LOW);  // Turn LED off
      value = 179;
      servo.write(value);
      delay(500);
    }
  }
}