param (
    [Parameter(Mandatory = $True, HelpMessage = "Specifies your Visual Studio version (2010 or 2012).")]
    [ValidateSet("2010", "2012")]
    [string] $Version
)

$Location = Split-Path -Parent $MyInvocation.MyCommand.Path
$Path = Join-Path -Path $Location -ChildPath Helpers.psm1

Import-Module $Path

Import-Variables -Version $Version

& certmgr.exe -add -c (Join-Path -Path $Location -ChildPath "RootCertificate.cer") -s -r localMachine root
Try-Exit

& sn -i (Join-Path -Path $Location -ChildPath "SigningKey.pfx") VS_KEY_CE727BD618DA74A9
Try-Exit

Read-Host -Prompt "Press any key to continue..."
# SIG # Begin signature block
# MIIGAwYJKoZIhvcNAQcCoIIF9DCCBfACAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQU2BRqU9jNztb1bF8ia6rsqsZu
# B16gggN8MIIDeDCCAmCgAwIBAgIQHYfZknm5sJVIr3/iJRKxVjANBgkqhkiG9w0B
# AQ0FADA8MTowOAYDVQQDEzFBbGVrc2FuZHIgVmlzaG55YWtvdiAtIFRlbXBvcmFy
# eSByb290IGNlcnRpZmljYXRlMB4XDTEzMDgyMjE0MzcxMloXDTM5MTIzMTIzNTk1
# OVowKjEoMCYGA1UEAxMfSW50ZXJvcCAtIFRlbXBvcmFyeSBjZXJ0aWZpY2F0ZTCC
# ASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAIrgVLf11h29zQz3967k9JMs
# gZ+C4Y1vPST23L+gGc5Z/XzC1BCXL5WGwVK3I/TRUlM6urPV4IYM3U+LeECQJEPw
# C2jcJhbVpKTlQfPuaM9mU8lcz3/rl0Xr1I72HocJZNmXhv9srkINd2K2olGq2THs
# WMobQ00Fh95fOzNbe88tI7w0gueWhjnBN84hwUzrLbnxEDBSFlHeH8OTNkkZItGP
# kKAGpqcdbHt4X6zMW3BOXyXVQtmd0beeI0PUyRy4npuD5kNcH5AcOA1hT6vsqjdj
# 2FKgHUa3HZNP494jBmrAfZm/INzrt6SjumNvDwvoW9xJwyUtlv4fzJHMvpAcrysC
# AwEAAaOBhzCBhDATBgNVHSUEDDAKBggrBgEFBQcDAzBtBgNVHQEEZjBkgBBnVlWU
# Fuyg6Mb0/essbehVoT4wPDE6MDgGA1UEAxMxQWxla3NhbmRyIFZpc2hueWFrb3Yg
# LSBUZW1wb3Jhcnkgcm9vdCBjZXJ0aWZpY2F0ZYIQPtMhNrOL6YVORyl7mbhbxDAN
# BgkqhkiG9w0BAQ0FAAOCAQEARgMoTJqJolLkKvrYtRVomoHuDmtmp82jB3jfWpJE
# ssLe5+xfudkeA3Uk1DpYMXuzBLMsziW9koilbvtlu05CxITIhzMzQobddkh82xSo
# I1oCeFduhVaTNy8aHk8/tDAx7Xb6O5YMs5eA8R5zhAL98cYnxbwlD7FVbLDfUTji
# Lzb1PLXx3wHgSt4kI/XCBUw5bHZhF7QxO4Iw+iMx1M7RnAzl6py6tYeNDZTW0ZSC
# 0iihSdIg07waSBZN7f/ofqnWJByNYW8taJ6F57+PSf8M38M0YYJh4tDqaMDU/8wh
# ufp7OAm2V3en5jkHfAvfN6znUvc74nxbXWgs4RjjAZxQnzGCAfEwggHtAgEBMFAw
# PDE6MDgGA1UEAxMxQWxla3NhbmRyIFZpc2hueWFrb3YgLSBUZW1wb3Jhcnkgcm9v
# dCBjZXJ0aWZpY2F0ZQIQHYfZknm5sJVIr3/iJRKxVjAJBgUrDgMCGgUAoHgwGAYK
# KwYBBAGCNwIBDDEKMAigAoAAoQKAADAZBgkqhkiG9w0BCQMxDAYKKwYBBAGCNwIB
# BDAcBgorBgEEAYI3AgELMQ4wDAYKKwYBBAGCNwIBFTAjBgkqhkiG9w0BCQQxFgQU
# us0ErI3ql5odh6INb6uyaK6se9swDQYJKoZIhvcNAQEBBQAEggEANTYjpFOSUl84
# ChEcD5FXzV2YQeIbb+pEFsbqgrxBIBFS9rQWGADE5IbMsbgkz00JqtTpZPSFbCdj
# qbpAwR0Kt4Kblpa35D3dmIq+QHpUNXDRsWZH0OADSqjy2lsQzr35YEhd08ou1M7X
# Ti4JqhZf2OUDcD91XpNR7w+qxwb3D3wAkxbh02mvvQN6vOplPEBd7g4EiwVNHWj/
# sC4enThUWOkgsgwr21D0+L7ah+0VgD4OO1AdDfoPiTjK/dg8BhoGAlzQ+fIJqJu7
# EdN+QV3FRLZDDvXWAm1MbbqUvbljciClPjnuVZeR5vh0F+TJZYmI4Xyy7Uv405Zp
# 5drCHMf26w==
# SIG # End signature block
