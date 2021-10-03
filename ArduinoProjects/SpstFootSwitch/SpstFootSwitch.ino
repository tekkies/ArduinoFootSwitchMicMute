void printBanner()
{
  Serial.print(F("Tekkies Foot Switch:SPST:v1.00\r\n"));
}

void setup() {
  pinMode(11, INPUT);
  digitalWrite(11, HIGH);
  pinMode(13, OUTPUT);
  digitalWrite(13, LOW);
  Serial.begin(9600);
  //printBanner();
}



void loop() {
  int lastMicHot=-1;
  int micHot = -1;
  while(true) 
  {

      int chars = Serial.peek();
      if(chars != -1)
      {
        do
        {
          Serial.read();
          chars = Serial.peek();
        } while(chars != -1);
        printBanner();
      }


      micHot = !digitalRead(11);

      if(micHot != lastMicHot) {
        digitalWrite(13, micHot);
        Serial.print(micHot ? "H" : "M");
        lastMicHot = micHot;
        delay(100);
    }
  }
}
