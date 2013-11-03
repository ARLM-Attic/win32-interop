# Try exit if error occurred
function Try-Exit
{
    param (
        [Parameter(Position = 0, Mandatory = $False)]
        [int]$ExitCode = $LASTEXITCODE
    )
    
    if ($ExitCode -ne 0)
    {
        Write-Host "Error occurred"
        Read-Host -Prompt "Press any key to continue..."
        exit $ExitCode
    }
}

# Import environment variables from specified environment
function Invoke-Environment
{
    param (
        [Parameter(Position = 0, Mandatory = $True)]
        [string]$Command,

        [Parameter(Position = 1, Mandatory = $False)]
        [switch]$Force
    )
    
    $stream = ($temp = [IO.Path]::GetTempFileName())
    $operator = if ($Force) {"'&'"} else {"'&&'"}
    
    
    Invoke-Expression -Command "$env:ComSpec /c $Command > `"$stream`" 2>&1 $operator set"
    foreach($_ in Get-Content -LiteralPath $temp) {
        if ($_ -match '^([^=]+)=(.*)') {
            [System.Environment]::SetEnvironmentVariable($matches[1], $matches[2])
        }
    }
    
    Remove-Item -LiteralPath $temp
}

# Import Visual Studio environment variables
function Import-Variables
{
    param (
        [Parameter(Mandatory = $True, HelpMessage = "Specifies your Visual Studio version (2010 or 2012). Default is 2012.")]
        [ValidateSet("2010", "2012")]
        [string] $Version
    )

    switch ($Version)
    {
        "2010"  { $Variables = Join-Path $env:VS100COMNTOOLS -ChildPath "..\..\VC\vcvarsall.bat" -Resolve }
        "2012" { $Variables = Join-Path $env:VS110COMNTOOLS -ChildPath "..\..\VC\vcvarsall.bat" -Resolve }
    }

    Invoke-Environment -Command "`"$Variables`" $env:PROCESSOR_ARCHITECTURE"
}
# SIG # Begin signature block
# MIIGAwYJKoZIhvcNAQcCoIIF9DCCBfACAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQUW2nyInTCgtixTnQnZd1fdeX+
# fp+gggN8MIIDeDCCAmCgAwIBAgIQHYfZknm5sJVIr3/iJRKxVjANBgkqhkiG9w0B
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
# L39xALpTHyTqbe6DRKR03SZJhEgwDQYJKoZIhvcNAQEBBQAEggEAKeC8XJKrsJMz
# 14DgSLmZPZZoqCPfDTvyfIVkAY5q19giqLudenEhvwmgYskzetGbsIzclM8QtSiW
# 0xJ1O4YMtoJwiPkMAcFIHuGbvB9RXes63++PbifBSt5VTyTfOMX/EiKZnbiRypit
# Ig9JMvQsqHqiuVQQE5BJGHPspSTkShZ+7VX6CpxYBxKjgDyvJLawsxS2/9M2yZl5
# M2TYE0AowBgPhu2EwJB66eVOenB0DyvWGjgTgVsm0c2gGf9PJIel/DPH5kKJ2ZXw
# pJf1cLwbPjHrlBkwEUXmi1BJY3vKNsTpjPHYtuMVYflavPYU9xMlpQ7L0NdSu8ke
# IP7AiK+fVg==
# SIG # End signature block
