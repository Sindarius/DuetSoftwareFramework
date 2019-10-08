; Configuration file for Duet WiFi (firmware version 1.20 or newer)
; executed by the firmware on start-up
;
; generated by RepRapFirmware Configuration Tool on Thu Oct 26 2017 17:58:26 GMT+0200 (CEST)

; General preferences
M111 P3 S1
M111 P16 S1
G21 ; Work in millimetres
G90 ; Send absolute coordinates...
M83 ; ...but relative extruder moves
M555 P2 ; Set firmware compatibility to look like Marlin
; Automatic saving after power loss is not enabled
M208 X0 Y1 Z0 S1 ; Set axis minima
M208 X210 Y210 Z900 S0 ; Set axis maxima

; Endstops
M574 X1 S2 ; Use Z-probe for the X axis
M574 P"io1.in" Y1 S1 ; Define active high microswitches
M574 Z1 S2 ; Use Z-probe for the Z axis
M558 C"io8.in+io8.out" P2 H3 F120 T9000 ; Set Z probe type to modulated, the axes for which it is used and the probe + travel speeds
G31 P500 X-35 Y0 Z0.1 ; Set Z probe trigger value, offset and trigger height
M557 X15:175 Y15:175 S40 ; Define mesh grid

; Drives
M569 P0 S0 ; Drive 0 goes backwards
M569 P1 S1 ; Drive 1 goes forwards
M569 P2 S1 ; Drive 2 goes forwards
M569 P3 S1 ; Drive 3 goes forwards
M569 P4 S1 ; Drive 4 goes forwards
M350 X16 Y16 Z16 E16:16 I1 ; Configure microstepping with interpolation
M92 X87.489 Y87.489 Z4266.667 E428:423 ; Set steps per mm
M566 X750 Y750 Z30 E750:750 ; Set maximum instantaneous speed changes (mm/min)
M203 X15000 Y15000 Z270 E6000:6000 ; Set maximum speeds (mm/min)
M201 X900 Y900 Z10 E900:900 ; Set accelerations (mm/s^2)
M906 X800 Y1000 Z800 E1000:1000 I30 ; Set motor currents (mA) and motor idle factor in per cent
M84 S60 ; Set idle timeout

; Heaters
M950 H0 C"out0"
M950 H1 C"out1"
M950 H2 C"out2"
M143 S260 ; Set maximum heater temperature to 260C
M305 P0 T10000 B3988 C0 ; Set thermistor + ADC parameters for heater 0
M301 H1 S0.87 ; Set heater 1 output scale factor to 87%
M305 P1 T100000 B4138 C0 ; Set thermistor + ADC parameters for heater 1
M301 H2 S0.87 ; Set heater 2 output scale factor to 87%
M305 P2 T100000 B4138 C0 ; Set thermistor + ADC parameters for heater 2

; Tools
M563 P0 D0 H1 ; Define tool 0
G10 P0 X-9.45 Y-0.1 Z0 ; Set tool 0 axis offsets
G10 P0 R0 S0 ; Set initial tool 0 active and standby temperatures to 0C
M563 P1 D1 H2 ; Define tool 1
G10 P1 X9.45 Y0.1 Z0 ; Set tool 1 axis offsets
G10 P1 R0 S0 ; Set initial tool 1 active and standby temperatures to 0C

; Network
M550 POrmerod ; Set machine name
M552 S1 ; Enable network
; Access point is configured manually via M587 by the user
;M586 P0 S1 ; Enable HTTP
;M586 P1 S1 ; Enable FTP
;M586 P2 S1 ; Enable Telnet

; Fans
M950 F3 C"out7" Q500
M950 F4 C"!out8+out4.tach" Q25000
M106 P3 S1 H1:2 T45 ; Set fan 3 value, PWM signal inversion and frequency. Thermostatic control is turned on
M106 P4 S0.35 H-1 ; Set fan 4 value, PWM signal inversion and frequency. Thermostatic control is turned off

; Custom settings
M572 D0 S0.275 ; Set bowden extruder elasticity compensation for E0
M572 D1 S0.275 ; Set bowden extruder elasticity compensation for E1
M207 S4.0 F2400 Z0.075 ; Set firmware retraction parameters

; Scanner support (debug)
;M750

; Miscellaneous
;M501 ; Load saved parameters from non-volatile memory

; Set up DHT sensor on channels 101-102
;M305 P101 X405 S"DHT temperature" T11
;M305 P102 X455 S"DHT humidity [%]" T11