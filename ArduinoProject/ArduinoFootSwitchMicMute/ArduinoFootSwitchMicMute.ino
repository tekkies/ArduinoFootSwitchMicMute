void setup() {
  // put your setup code here, to run once:
  pinMode(10, INPUT);           // set pin to input
  digitalWrite(10, HIGH);       // turn on pullup resistors
  pinMode(11, INPUT);           // set pin to input
  digitalWrite(11, HIGH);       // turn on pullup resistors
  Serial.begin(9600);
  Serial.println(F("ArduinoFootSwitchMicMute"));
}

void loop() {
  // put your main code here, to run repeatedly:
  int p10;
  int p11;
  int lastNet=-1;
  while(true) {
    p10 = digitalRead(10);
    p11 = digitalRead(11);
    int net = (p10+p11*2);
    if((net == 1) || (net == 2)) {
      if(net != lastNet) {
        Serial.print(net);
        lastNet = net; 
      }
    }
  }
}
