function CalcMOD(o2, ppo2, isFreshWater) {
    var atmDepth = 34;
    var isFresh = GetRadioValue(isFreshWater);

    if (isFresh == "false") {
        atmDepth = 33;
    }
    var mod = (ppo2 / (o2 / 100) - 1) * atmDepth;
    return Math.round(mod);
}

function CalcBoth(formVar) {
    alert("test");
    formVar.MOD.value = CalcMOD(formVar.O2.value, formVar.PPO2.value, formVar.IsFreshWater);
    if (formVar.HE.value > 0) {
        formVar.END.value = CalcEND(formVar.HE.value, formVar.MOD.value, GetRadioValue(formVar.IsFreshWater));
    }
    else {
        formVar.END.value = formVar.MOD.value;
    }
    return;
}

function CalcEND(he, depth, isFreshWater) {
    alert("Message");
    var end;
    var atmDepth = 33;
    if (isFreshWater == "true") atmDepth = 34;
    //end = ( (1 - (he / 100)) * (depth + atmDepth)) - atmDepth;
    var1 = 1 - (he / 100);
    var2 = parseFloat(depth) + atmDepth;
    end = (var1 * var2) - atmDepth;
    return Math.round(end);
}

function CalcEndWithO2(formVar) {
    alert("test");
    if (parseInt(formVar.He) + parseInt(formVar.O2) > 100) {
        Alert("O2 + Helium can't exceed 100%");
    } else {
        formVar.END.value = CalcEND(formVar.HE.value, formVar.Depth.value, true));
        alert(fornVar.End.value);
    }
    return;
}

function GetRadioValue(field) {
    var value = "";
    for (i = 0; i < field.length; i++) {
        if (field[i].checked) {
            value = field[i].value;
        }
    }
    return value;
}