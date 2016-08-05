# HighPrecisionTimer
A high precision timer in .NET

There are often times where people would like regularly spaced events at a resolution greater than C# and Windows 
can normally provide (~15ms). This is a limitation of the standard timer mechanisms built into the Windows OS. 
There do exist high precision timing APIs for multimedia applications, but these are not exposed via .NET APIs.
For applications that need timer event precision on the order of 1ms, these APIs are useful. This library is a quick 
example of using the [Multimedia Timer API](http://msdn.microsoft.com/en-us/library/windows/desktop/dd743609(v=vs.85).aspx).

I will note that this API changes system wide settings that can degrade system performance (esp battery life), so buyer 
beware. Since Windows is not a real-time OS, the load on your system may cause the MM timer be delayed resulting 
in gaps of 100 ms that contain 100 events in quick succession, rather than 100 events spaced 1 ms apart. 
Some additional reading on [MM timers](https://blogs.msdn.microsoft.com/mediasdkstuff/2009/07/02/why-are-the-multimedia-timer-apis-timesetevent-not-as-accurate-as-i-would-expect/).

I originally posted the code as an [answer](http://stackoverflow.com/a/24843946/517852) to a question on Stack Overflow