from __future__ import annotations

import json
from pathlib import Path
from xml.etree import ElementTree


ROOT = Path(__file__).resolve().parents[1]


raw_settings = (ROOT / "appsettings.json").read_text(encoding="utf-8-sig")
settings = json.loads(
    "\n".join(
        line for line in raw_settings.splitlines() if not line.lstrip().startswith("//")
    )
)

sensitive_values = {
    "ConnectionStrings.DefaultConnection": settings["ConnectionStrings"]["DefaultConnection"],
    "Auth0.ClientSecret": settings["Auth0"]["ClientSecret"],
    "AWS.AccessKey": settings["AWS"]["AccessKey"],
    "AWS.SecretKey": settings["AWS"]["SecretKey"],
}
configured = [name for name, value in sensitive_values.items() if value]
if configured:
    raise SystemExit(f"Tracked configuration contains credentials: {configured}")

project = ElementTree.parse(ROOT / "ShibAPI.csproj")
packages = {
    element.attrib["Include"]: element.attrib.get("Version", "")
    for element in project.findall(".//PackageReference")
}
if "Npgsql.EntityFrameworkCore.PostgreSQL.Design" in packages:
    raise SystemExit("The obsolete Npgsql design-time package must not be restored")

print("Repository configuration checks passed.")
