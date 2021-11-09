var tableSelector = $("table.table-striped tr:last");

function appendRows(temperature) {
  tableSelector.after(
    `<tr>
    <td></td>
    <td>${temperature.clientId}</td>
    <td>${temperature.value}</td>
    <td>${Date.parse(temperature.receivedTime)}</td>
    </tr>`
  );
}
