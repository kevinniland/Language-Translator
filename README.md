<h3><b>Student:</b></h3> <i>Kevin Niland</i>
<h3><b>Student number/email:</b></h3> <i>G00342279</i>
<h3><b>Module:</b></h3> <i>Mobile Applications Development 2</i>
<h3><b>Build version:</b></h3> <i>1.0.0</i>

<h2>Project Statement</h2>
<p>Create a cross platform app using Xamarin and Visual Studio. This should deploy to Android, iOS and Windows (as a Universal Windows Platform application). The application should be designed with a clear purpose that is easily grasped by the user. You need to incorporate the following elements:</p>

<h2>Aim</h2>
<ul>
<li>Well-designed UI that is fit for purpose and provides a good user experience.</li>
<li>Uses some form of data storage. Using settings is not enough, this must be either file
storage using a JSON format or a database system (local) to manage the data for the
application. The data that is managed can include settings that you read at start up and
maintain based on the user preferences.</li>
<li>Demonstrates appropriate use of the sensors/hardware available on UWP capable devices
These include accelerometer, gyroscope, location services, sound, network service
(connect to server for data), camera, multi touch gestures, pictures, documents.
It is not necessary to use multiple sensors, but those used should be documented
and justified.</li>
<li>The app must be more than a simple information app.</li>
<li>You need to create a test plan as part of the submission to show how you have tested the
application and tracked any issues that have arisen from this testing process.</li>
</ul>

<h2>Tech/Framework?Language(s) used:</h2>
<ul>
  <li>C#/Xamarin (Cross-Platform Application)</li>
  <li>Visual Studio 2017</li>
  <li><a href="https://www.newtonsoft.com/json">Newtonsoft.Json</a></li>
  <li><a href="https://www.nuget.org/packages/Plugin.CurrentActivity/">Plugin.CurrentActivity</a></li>
  <li><a href="http://restsharp.org/">RestSharp</a></li>
  <li><a href="https://www.nuget.org/packages/Xamarin.Plugin.FilePicker/1.4.0-beta">Xam.Plugin.FilePicker</a></li>
  <li><a href="https://www.nuget.org/packages/Xam.Plugins.TextToSpeech">Xam.Plugins.TextToSpeech</a></li>
  <li><a href="https://tech.yandex.com/translate/">Yandex API</a></li>
</ul>

<h2>What it does</h2>
<p>The application is a simple language translator that takes in a user's input and translates the input into a desired language. The user input can be in three different forms; simple text the user has inputted, text from a file that the user has read in, or spoken text.</p>

<h2>How to use the application</h2>
<ul>
  <li>Clone or download the project to your desired directory.</li>
  <li>Open Visual Studio.</li>
  <li>Open the solution (LanguageTranslator.sln) <b>(Due to the way Visual Studio handles paths, the project may need to be opened, rebuilt/built numerous times).</b></li>
  <li>The application has been built to work both as an UWP (Universal Windows Platform) application and as an Android application. To launch the application on Windows, simply select 'LanguageTranslator.UWP' from the 'Startup Projects' dropdown. To deploy the application to your Android phone, you must first enable 'Developer Settings' on your phone:</li>
  <ul>
    <li>Go to Settings, and click on 'About Phone'.</li>
    <li>Tap on 'Build Number' several times. You should be notified that you are now a developer.</li>
    <li>Next, search your Settings for 'USB Debugging'.</li>
    <li>Enable USB Debugging from here.</li>
  </ul>
  You should now be able to connect your phone to your PC via a USB cable and simply select 'LanguageTranslator.Android' from the 'Startup Projects' dropdown. Your phone should be recognised and you will be able to build the application to your phone.
</ul>

<h2>Please refer to the wiki for research conducted, problems encountered, etc.</h2>
