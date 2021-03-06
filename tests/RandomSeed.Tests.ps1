Describe "Set-CNTKRandomSeed" {

  It "can fix random numbers generated by CNTK" {

    Set-CNTKRandomSeed 1234

    Get-CNTKRandomSeed | Should -Be 1234
    Test-CNTKRandomSeedFixed | Should -Be $true

    $r1 = (cntk.normalrandom 1 float).Invoke().ToArray()[0]
    $r2 = (cntk.normalrandom 1 float).Invoke().ToArray()[0]
    $r3 = (cntk.normalrandom 1 float).Invoke().ToArray()[0]

    $r1 | Should -Be $r2
    $r1 | Should -Be $r3

    Reset-CNTKRandomSeed

    Test-CNTKRandomSeedFixed | Should -Be $false
  }

  It "can fix random numbers generated by cntk.noisesampler" {

    Set-CNTKRandomSeed 1234
    $s = cntk.noisesampler "random" 1 1
    $r1 = $s.GetNextMinibatch()["random"].ToArray()

    Set-CNTKRandomSeed 5678
    $s = cntk.noisesampler "random" 1 1
    $r2 = $s.GetNextMinibatch()["random"].ToArray()

    Set-CNTKRandomSeed 1234
    $s = cntk.noisesampler "random" 1 1
    $r3 = $s.GetNextMinibatch()["random"].ToArray()

    $r1 | Should -Not -Be $r2
    $r1 | Should -Be $r3
  }

}
