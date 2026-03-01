param(
  [Parameter(Mandatory = $true)]
  [ValidateSet("identity","farm","telemetry","ingestion","all")]
  [string]$Service,

  [Parameter(Mandatory = $false)]
  [string]$Tag = "",

  [Parameter(Mandatory = $false)]
  [string]$Namespace = "fiapfase5"
)

$ErrorActionPreference = "Stop"

function Info($msg) { Write-Host "==> $msg" -ForegroundColor Cyan }
function Ok($msg)   { Write-Host "[OK] $msg" -ForegroundColor Green }
function Fail($msg) { Write-Host "[ERRO] $msg" -ForegroundColor Red; exit 1 }

function Get-TagOrDefault([string]$t) {
  if (![string]::IsNullOrWhiteSpace($t)) { return $t }
  return (Get-Date -Format "yyyyMMdd-HHmmss")
}

function DockerfilePath([string]$svc) {
  $svcPascal = switch ($svc) {
    "identity"  { "Identity" }
    "farm"      { "Farm" }
    "telemetry" { "Telemetry" }
    "ingestion" { "Ingestion" }
    default     { throw "Servico invalido: $svc" }
  }
  return ".\src\$svcPascal\$svcPascal.Api\Dockerfile"
}

function BuildImage([string]$svc, [string]$tag) {
  $df = DockerfilePath $svc
  if (!(Test-Path $df)) { Fail "Dockerfile nao encontrado: $df" }

  $image = ("fiap/" + $svc + ":" + $tag)
  Info "Buildando imagem $image (Dockerfile: $df)"
  docker build -t $image -f $df . | Write-Host
  Ok "Imagem gerada: $image"
  return $image
}

function DeployImage([string]$svc, [string]$image, [string]$ns) {
  Info "Atualizando deployment/$svc no namespace $ns para $image"
  kubectl set image -n $ns "deploy/$svc" "$svc=$image" | Write-Host

  Info "Aguardando rollout..."
  kubectl rollout status -n $ns "deploy/$svc" | Write-Host

  Ok "Deploy concluido: $svc -> $image"
}

function EnsureTools() {
  Info "Verificando Docker e kubectl..."
  docker version | Out-Null
  kubectl version --client | Out-Null
  Ok "Ferramentas OK"
}

function EnsureNamespace([string]$ns) {
  Info "Contexto atual do kubectl:"
  kubectl config current-context | Write-Host
  Info "Checando namespace $ns..."
  $exists = kubectl get ns $ns -o name 2>$null
  if (!$exists) {
    Info "Namespace nao existe, criando..."
    kubectl create namespace $ns | Write-Host
  }
  Ok "Namespace OK: $ns"
}

try {
  EnsureTools
  EnsureNamespace $Namespace

  $tagFinal = Get-TagOrDefault $Tag

  $targets = @()
  if ($Service -eq "all") {
    $targets = @("identity","telemetry","ingestion","farm")
  } else {
    $targets = @($Service)
  }

  foreach ($svc in $targets) {
    Info "---- Deploy: $svc ----"
    $img = BuildImage $svc $tagFinal
    DeployImage $svc $img $Namespace
  }

  Ok "Tudo finalizado."
}
catch {
  Fail $_.Exception.Message
}