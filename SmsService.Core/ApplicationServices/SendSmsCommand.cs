using System.Text.Json;

namespace Tgs.SmsService.ApplicationServices;

public record SendSmsCommand(
    ShortMessageData ShortMessage,
    string VendorCode,
    JsonDocument VendorSettings);
