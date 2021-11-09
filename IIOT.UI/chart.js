function RenderChart(timeSeries, data) {
  $("canvas#chart").remove();
  $("div#chart-section").append(
    `<canvas id="chart" class="animated fadeIn"width="120" height="20"></canvas>`
  );
  const ctx = document.getElementById("chart").getContext("2d");
  let temperatureData = {
    labels: timeSeries,
    datasets: [
      {
        label: "Last 20 temperature values",
        data: data,
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

  let chartOptions = {
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
}
