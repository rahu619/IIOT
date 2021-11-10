const hub = "http://127.0.0.1:5000/temperaturehub";
const n_values = 20;
var temperatureArr = [];

const connection = new signalR.HubConnectionBuilder()
  .withUrl(hub)
  .configureLogging(signalR.LogLevel.Information)
  .build();

async function startHubCommunication() {
  try {
    await connection.start();
    console.log("SignalR Connected.");
  } catch (err) {
    console.log(`error : ${err}`);
    setTimeout(startHubCommunication, 5000);
  }
}

connection.on("ReceiveMessage", (temperature) => {
  console.log(`Received temperature: ${temperature.value}`);

  temperatureArr.push(temperature);
  //taking the 'N' values latest alone
  let latestTemperatureArr = temperatureArr.slice(
    Math.max(temperatureArr.length - n_values, 0)
  );
  let valueArr = latestTemperatureArr.map((a) => a.value.replace(/\D+$/g, ""));
  let TimeArr = latestTemperatureArr.map((a) =>
    new Date(a.receivedTime).toLocaleTimeString()
  );

  RenderChart(TimeArr, valueArr);
  appendRows(temperature);
});

connection.onclose(async () => {
  console.log("disconnect!");
  await startHubCommunication();
});

// // Starting the connection.
// startHubCommunication();

// let latestTemperatureArr = temperatureArr.slice(
//   Math.max(temperatureArr.length - 7, 0)
// );
// let valueArr = latestTemperatureArr.map((a) => a.Value);
// let TimeArr = latestTemperatureArr.map((a) => a.ReceivedTime);
// RenderChart(TimeArr, valueArr);
