echo off
pip install pre-commit

git config core.commentchar ';'
pre-commit install -t {pre-commit,commit-msg}
