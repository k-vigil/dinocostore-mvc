let i = 0;

function addItem({ pid, description, price, quantity }) {
  let newRow = `
    <tr class="align-middle" data-index="${i}" data-subtotal="${price * quantity}" data-pid="${pid}" data-price="${price}" data-quantity="${quantity}">
      <td>${description}</td>
      <td>$${price}</td>
      <td>${quantity}</td>
      <td>$${price * quantity}</td>
      <td class="text-end">
        <button type="button" class="btn btn-outline-danger btn-sm" onclick="removeItem(${i})">
          <i>&times;</i>
        </button>
      </td>
    </tr>
  `;

  i++;
  document.getElementById("tbody-items").innerHTML += newRow;
  updateTotal();
}

function removeItem(index) {
  const row = document.querySelector(`#tbody-items [data-index="${index}"]`);
  row.remove();
  updateTotal();

  document.getElementById('inputs-here').innerHTML = "";
}

function updateTotal() {
  const rows = document.querySelectorAll("#tbody-items tr");
  let total = 0;

  rows.forEach(r => {
    total += parseFloat(r.getAttribute("data-subtotal"))
  })

  document.getElementById("total-text").textContent = total.toFixed(2);
  document.getElementById("total").value = total.toFixed(2);
}

document.getElementById("btnAddItem").addEventListener("click", e => {
  let pid = $("#products option:selected").attr("data-id");
  let description = $("#products option:selected").attr("data-desc");
  let price = $("#products option:selected").attr("data-price");
  let quantity = $("#quantity").val();

  if (pid == undefined || quantity == "")
    return alert("Add a valid item");

  addItem({
    pid,
    description,
    price,
    quantity
  });

  document.getElementById("quantity").value = "";
  $('#products').val("").trigger("change");
  $('#products').select2('open');
});

document.getElementById('btnSave').addEventListener('click', e => {
  let rows = document.querySelectorAll('#tbody-items tr');
  let html = '';
  let i = 0;

  rows.forEach(row => {
    html += `
      <div>
        <input type="hidden" name="Items[${i}].ProductId" value="${row.getAttribute('data-pid')}" />
        <input type="hidden" name="Items[${i}].Price" value="${row.getAttribute('data-price')}" />
        <input type="hidden" name="Items[${i}].Quantity" value="${row.getAttribute('data-quantity')}" />
      </div>
    `;
    i++
  });

  document.getElementById('inputs-here').innerHTML = html;
});
