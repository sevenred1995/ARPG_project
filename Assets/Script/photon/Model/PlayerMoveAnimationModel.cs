using UnityEngine;
using System.Collections;
using System;

public class PlayerMoveAnimationModel{
    public bool IsMove { get; set; }
    public string Time { get; set; }

    public void SetTime(DateTime dateTime) {
        Time = dateTime.ToString("yyyyMMddHHmmssffff");
    }
    public DateTime GetTime() {
        DateTime dt = DateTime.ParseExact(Time, "yyyyMMddHHmmssffff", System.Globalization.CultureInfo.InvariantCulture);
        return dt;
    }
}
