using System;

namespace ZPHelper.Captcha
{
    [Flags]
    public enum ErrorState
    {
        // @formatter:off
        Critical         = 0b1,
        LowBet           = 0b10,
        CaptchaNotReady = 0b100,
        Unknown          = 0b10000000
        // @formatter:on
    }
}
