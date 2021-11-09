const ctx = document.getElementById("myChart").getContext("2d");

//TODO: show last N temperature values

var temperatureData = {
  labels: ["0s", "10s", "20s", "30s", "40s", "50s", "60s", "70s", "80s"],
  datasets: [
    {
      label: "Recent temperatures",
      data: [0, 59, 75, 20, 20, 55, 40, 70, 90, 100],
      lineTension: 0,
      fill: false,
      tension: 0.1,
      borderColor: "rgb(75, 192, 192)",
      backgroundColor: "transparent",
      borderDash: [5, 5],
      pointBorderColor: "orange",
      pointBackgroundColor: "rgba(255,150,0,0.5)",
      pointRadius: 3,
      pointHoverRadius: 10,
      pointHitRadius: 30,
      pointBorderWidth: 2,
      //   pointStyle: "rectRounded",
    },
  ],
};

var chartOptions = {
  legend: {
    display: true,
    position: "top",
    labels: {
      boxWidth: 50,
      fontColor: "black",
    },
  },
};

const lineChart = new Chart(ctx, {
  type: "line",
  data: temperatureData,
  options: chartOptions,
});
