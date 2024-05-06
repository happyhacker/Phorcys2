
  var viewModel = {
    O2: ko.observable(21),
    He: ko.observable(0),
    Depth: ko.observable(),
    END: ko.observable()
  };

  function Calc() {
    if (parseInt(viewModel.He()) + parseInt(viewModel.O2()) > 100) {
      alert("O2 + Helium can't exceed 100%");
    } else {
      viewModel.END(CalcEND(viewModel.He(), viewModel.Depth(), true));
    }
  }
