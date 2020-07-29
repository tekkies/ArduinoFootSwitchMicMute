void setup() {
  pinMode(10, INPUT);
  digitalWrite(10, HIGH);
  pinMode(11, INPUT);
  digitalWrite(11, HIGH);
  pinMode(13, OUTPUT);
  digitalWrite(13, LOW);
  Serial.begin(9600);
  Serial.println(F("ArduinoFootSwitchMicMute v1.01"));
}

void loop() {
  int lastMicHot=-1;
  int micHot = -1;
  while(true) 
  {
    int pattern = (digitalRead(10)+digitalRead(11)*2);
    if(pattern == 1) 
    {
      micHot = 1;
    } 
    else if (pattern == 2) 
    {
      micHot = 0;
    }

    if(micHot != lastMicHot) {
      Serial.print(micHot ? "H" : "M");
      lastMicHot = micHot;
    }
  }
}
